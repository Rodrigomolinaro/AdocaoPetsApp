using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;

namespace AdocaoPetsApp.Views
{
    public partial class GerenciarPetsPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly ONG _ongLogada;

        public GerenciarPetsPage(DatabaseContext database, ONG ongLogada)
        {
            InitializeComponent();
            _database = database;
            _ongLogada = ongLogada;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarPets();
        }

        private async Task CarregarPets()
        {
            try
            {
                // 1. Tenta ir ao banco de dados com segurança
                var meusPets = await _database.Pets
                    .Where(p => p.IdOng == _ongLogada.IdOng)
                    .ToListAsync();

                // 2. Garante que a atualização da lista ocorre na thread de interface do Android
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    PetsCollectionView.ItemsSource = meusPets;

                    // Alerta de diagnóstico se a lista voltar vazia
                    if (meusPets == null || meusPets.Count == 0)
                    {
                        await DisplayAlert("Diagnóstico", $"Nenhum pet encontrado para a ONG ID: {_ongLogada.IdOng}", "OK");
                    }
                });
            }
            catch (Exception ex)
            {
                // 3. Intercepta o crash fatal e mostra o ERRO REAL no ecrã do celular
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Erro Oculto", $"O verdadeiro erro é: {erroReal}", "OK");
                });
            }
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var pet = botao?.CommandParameter as Pet;

            if (pet != null)
            {
                // Confirmação de segurança para evitar cliques acidentais
                bool confirmar = await DisplayAlert("Excluir", $"Tem a certeza que deseja remover o {pet.Nome} permanentemente do sistema?", "Sim", "Não");

                if (confirmar)
                {
                    _database.Pets.Remove(pet);
                    bool sucesso = await _database.SaveChangesAsync() > 0;

                    if (sucesso)
                    {
                        await CarregarPets(); // Atualiza a lista na hora, fazendo o pet sumir do ecrã
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Não foi possível excluir o pet.", "OK");
                    }
                }
            }
        }

        private async void OnEditarClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var pet = botao?.CommandParameter as Pet;

            if (pet != null)
            {
                // Substituímos o alerta pela abertura da nova tela, enviando o pet escolhido
                await Navigation.PushAsync(new EditarPetPage(_database, pet));
            }
        }
    }
}