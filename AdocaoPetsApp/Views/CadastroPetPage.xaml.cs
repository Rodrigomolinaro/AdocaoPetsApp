using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using System;
using Microsoft.Maui.Controls;

namespace AdocaoPetsApp.Views
{
    public partial class CadastroPetPage : ContentPage
    {
        private readonly DatabaseContext _database;

        // 1. Corrigido: Apenas uma definição e usando o tipo ONG!
        private readonly ONG _ongLogada;

        // 2. Corrigido: O construtor agora recebe ONG em vez de Usuario
        public CadastroPetPage(DatabaseContext database, ONG ongLogada)
        {
            InitializeComponent();
            _database = database;
            _ongLogada = ongLogada;
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // Pega a idade com segurança
            int idade = 0;
            int.TryParse(IdadeEntry.Text, out idade);

            var novoPet = new Pet
            {
                Nome = NomeEntry.Text,
                Especie = EspecieEntry.Text,
                Raca = RacaEntry.Text,
                Idade = idade,
                ImagemUrl = ImagemUrlEntry.Text,
                Porte = PorteEntry.Text,
                Sexo = SexoEntry.Text,
                Cor = CorEntry.Text,
                Descricao = DescricaoEntry.Text, // 3. Corrigido: A vírgula que faltava está aqui!
                IdUsuario = _ongLogada.IdOng // O Pet agora fica amarrado à ONG
            };

            _database.Pets.Add(novoPet);
            bool sucesso = await _database.SaveChangesAsync() > 0;

            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Pet cadastrado com sucesso!", "OK");

                // Limpa os campos do formulário para o próximo cadastro
                NomeEntry.Text = "";
                EspecieEntry.Text = "";
                RacaEntry.Text = "";
                IdadeEntry.Text = "";
                ImagemUrlEntry.Text = "";
                PorteEntry.Text = "";
                SexoEntry.Text = "";
                CorEntry.Text = "";
                DescricaoEntry.Text = "";
            }
            else
            {
                await DisplayAlert("Erro", "Ocorreu um erro ao salvar o pet.", "OK");
            }
        }

        private async void OnVerPetsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListagemPetsPage(_database, null));
        }

        private async void OnVerPedidosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolicitacoesPage(_database));
        }
    }
}