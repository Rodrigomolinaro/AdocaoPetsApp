using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace AdocaoPetsApp.Views
{
    public partial class ListagemPetsPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly Usuario _usuarioLogado; // Guarda quem está logado
        private List<Pet> _listaCompletaPets = new(); // Guarda todos os pets vindos do banco

        // O construtor recebe a conexão e o utilizador logado (null se for a ONG a ver a lista)
        public ListagemPetsPage(DatabaseContext database, Usuario usuarioLogado = null)
        {
            InitializeComponent();
            _database = database;
            _usuarioLogado = usuarioLogado;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Busca todos os pets disponíveis no MySQL
            _listaCompletaPets = await _database.ObterPetsDisponiveisAsync();

            // Exibe todos por padrão ao abrir a tela
            PetsCollectionView.ItemsSource = _listaCompletaPets;
        }

        // --- MÉTODOS DE FILTRAGEM ---

        private void OnFiltrarTodosClicked(object sender, EventArgs e)
        {
            PetsCollectionView.ItemsSource = _listaCompletaPets;
        }

        private void OnFiltrarCachorroClicked(object sender, EventArgs e)
        {
            // Filtra por espécie
            PetsCollectionView.ItemsSource = _listaCompletaPets
                .Where(p => p.Especie != null && p.Especie.Equals("Cachorro", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private void OnFiltrarGatoClicked(object sender, EventArgs e)
        {
            // Filtra por espécie
            PetsCollectionView.ItemsSource = _listaCompletaPets
                .Where(p => p.Especie != null && p.Especie.Equals("Gato", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // --- BOTÃO DE ADOTAR ---

        private async void OnAdotarClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var petSelecionado = botao?.CommandParameter as Pet;

            // Validação 1: O botão conseguiu ler os dados do pet?
            if (petSelecionado == null)
            {
                await DisplayAlert("Erro", "O sistema não conseguiu identificar qual é o pet deste cartão. Verifique o XAML.", "OK");
                return;
            }

            // Validação 2: É um adotante logado ou é a ONG?
            if (_usuarioLogado == null)
            {
                await DisplayAlert("Acesso Restrito", "Apenas Adotantes registados podem solicitar adoções.", "OK");
                return;
            }

            // Tentativa de gravação no banco de dados MySQL
            try
            {
                bool sucesso = await _database.SolicitarAdocaoAsync(_usuarioLogado.IdUsuario, petSelecionado.IdPet);

                if (sucesso)
                {
                    await DisplayAlert("🐾 Solicitação Enviada!",
                        $"O seu pedido para adotar o(a) {petSelecionado.Nome} foi registado com sucesso!\n\nA ONG responsável entrará em contacto em breve.",
                        "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "Você já enviou uma solicitação para este pet.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Mostra exatamente o erro que o banco de dados devolver
                await DisplayAlert("Erro no Banco de Dados", ex.Message, "OK");
            }
        }
        private async void OnMinhasAdocoesClicked(object sender, EventArgs e)
        {
            if (_usuarioLogado == null)
            {
                await DisplayAlert("Aviso", "Apenas usuários logados podem ver suas adoções.", "OK");
                return;
            }

            await Navigation.PushAsync(new MinhasAdocoesPage(_database, _usuarioLogado));
        }

        // --- BOTÃO DE SAIR ---

        private void OnSairClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage(_database));
        }
    }
}