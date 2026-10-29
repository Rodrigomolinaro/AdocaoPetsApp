using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel; // Necessário para proteção da Thread de UI

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
            try
            {
                // Busca a lista de pedidos pendentes na nuvem
                var solicitacoes = await _database.ObterSolicitacoesPendentesAsync();

                // Força a renderização segura na tela principal do Android
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SolicitacoesCollectionView.ItemsSource = solicitacoes;
                });
            }
            catch (Exception ex)
            {
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Erro de Conexão", $"Falha ao carregar pedidos: {erroReal}", "OK");
                });
            }
        }

        private async void OnAprovarClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var solicitacao = botao?.CommandParameter as SolicitacaoDTO;

            if (solicitacao == null)
            {
                await DisplayAlert("Erro", "Não foi possível identificar o pedido selecionado.", "OK");
                return;
            }

            bool confirmacao = await DisplayAlert("Aprovar Adoção",
                $"Deseja aprovar a adoção de {solicitacao.NomePet} para {solicitacao.NomeAdotante}?",
                "Sim", "Não");

            if (confirmacao)
            {
                try
                {
                    // Vai ao MySQL e aprova a adoção, mudando o status do pet
                    bool sucesso = await _database.AprovarAdocaoAsync(solicitacao.IdAdocao, solicitacao.IdPet);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Adoção aprovada! O pet foi retirado da lista pública.", "OK");
                        await CarregarSolicitacoes(); // Recarrega a lista para remover o cartão da tela
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Houve um problema ao aprovar a adoção. Tente novamente.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    await DisplayAlert("Erro no Banco", $"Falha ao aprovar: {erroReal}", "OK");
                }
            }
        }
    }
}