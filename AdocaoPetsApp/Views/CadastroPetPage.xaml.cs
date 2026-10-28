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
            if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(EspecieEntry.Text))
            {
                await DisplayAlert("Aviso", "O Nome e a Espécie do pet são obrigatórios!", "OK");
                return;
            }

            int.TryParse(IdadeEntry.Text, out int idade);

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
                Descricao = DescricaoEntry.Text,
                IdOng = _ongLogada.IdOng,
                Status = "Disponível", // Define o pet visível para adoção
                Adotado = false // Confirma que não foi adotado (tinyint no MySQL)
            };

            try
            {
                _database.Pets.Add(novoPet);
                bool sucesso = await _database.SaveChangesAsync() > 0;

                if (sucesso)
                {
                    await DisplayAlert("Sucesso", "Pet cadastrado com sucesso!", "OK");

                    // Envia a ONG diretamente para a tela atualizada com a lista de pets
                    await Navigation.PushAsync(new GerenciarPetsPage(_database, _ongLogada));
                }
            }
            catch (Exception ex)
            {
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await DisplayAlert("Erro do Banco", erroReal, "OK");
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