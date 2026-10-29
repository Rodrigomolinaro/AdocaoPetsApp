using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using Microsoft.EntityFrameworkCore; // Necessário para a busca blindada do Entity Framework
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel; // Necessário para o MainThread

namespace AdocaoPetsApp.Views
{
    public partial class ListagemPetsPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly Usuario _usuarioLogado;
        private List<Pet> _listaCompletaPets = new();

        public ListagemPetsPage(DatabaseContext database, Usuario usuarioLogado = null)
        {
            InitializeComponent();
            _database = database;
            _usuarioLogado = usuarioLogado;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Busca direta blindada: puxa apenas os que estão explicitamente disponíveis
                var petsDoBanco = await _database.Pets
                    .Where(p => p.Status == "Disponível" && p.Adotado == false)
                    .ToListAsync();

                // Atualiza a interface gráfica na Thread principal do Android
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _listaCompletaPets = petsDoBanco;
                    PetsCollectionView.ItemsSource = _listaCompletaPets;
                });
            }
            catch (Exception ex)
            {
                // Intercepta a falha fatal e mostra no ecrã sem fechar o app
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Erro de Conexão", $"Falha ao carregar a vitrine: {erroReal}", "OK");
                });
            }
        }

        // --- MÉTODOS DE FILTRAGEM ---

        private void OnFiltrarTodosClicked(object sender, EventArgs e)
        {
            PetsCollectionView.ItemsSource = _listaCompletaPets;
        }

        private void OnFiltrarCachorroClicked(object sender, EventArgs e)
        {
            PetsCollectionView.ItemsSource = _listaCompletaPets
                .Where(p => p.Especie != null && p.Especie.Equals("Cachorro", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private void OnFiltrarGatoClicked(object sender, EventArgs e)
        {
            PetsCollectionView.ItemsSource = _listaCompletaPets
                .Where(p => p.Especie != null && p.Especie.Equals("Gato", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // --- BOTÃO DE ADOTAR ---

        private async void OnAdotarClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var petSelecionado = botao?.CommandParameter as Pet;

            if (petSelecionado == null)
            {
                await DisplayAlert("Erro", "O sistema não conseguiu identificar qual é o pet deste cartão.", "OK");
                return;
            }

            if (_usuarioLogado == null)
            {
                await DisplayAlert("Acesso Restrito", "Apenas adotantes logados podem solicitar adoções.", "OK");
                return;
            }

            try
            {
                // Dispara o pedido no banco de dados usando o método do seu DatabaseContext
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
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await DisplayAlert("Erro no Banco de Dados", erroReal, "OK");
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