using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using System;
using System.Threading.Tasks;

namespace AdocaoPetsApp.Views
{
    public partial class SolicitacoesPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public SolicitacoesPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarSolicitacoes();
        }

        private async Task CarregarSolicitacoes()
        {
            // Busca a lista de pedidos pendentes
            var solicitacoes = await _database.ObterSolicitacoesPendentesAsync();
            SolicitacoesCollectionView.ItemsSource = solicitacoes;
        }

        private async void OnAprovarClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var solicitacao = botao?.CommandParameter as SolicitacaoDTO;

            if (solicitacao != null)
            {
                // Pede confirmação à ONG
                bool confirmacao = await DisplayAlert("Aprovar Adoção",
                    $"Deseja aprovar a adoção de {solicitacao.NomePet} para {solicitacao.NomeAdotante}?",
                    "Sim", "Não");

                if (confirmacao)
                {
                    // Vai ao MySQL e aprova a adoção, mudando o status do pet
                    bool sucesso = await _database.AprovarAdocaoAsync(solicitacao.IdAdocao, solicitacao.IdPet);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Adoção aprovada! O pet foi retirado da lista pública.", "OK");
                        await CarregarSolicitacoes(); // Recarrega a lista para sumir com este cartão
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Houve um problema ao aprovar a adoção. Tente novamente.", "OK");
                    }
                }
            }
        }
    }
}