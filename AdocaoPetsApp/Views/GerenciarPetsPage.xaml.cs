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
            await CarregarMeusPetsAsync();
        }

        private async Task CarregarMeusPetsAsync()
        {
            // Busca apenas os pets que pertencem ao ID desta ONG e que ainda não foram adotados
            var listaPets = await _database.ObterPetsPorOngAsync(_ongLogada.IdOng);

            // Filtra para exibir apenas os que não estão adotados na lista de gestão
            MeusPetsCollectionView.ItemsSource = listaPets.Where(p => p.Adotado != true);
        }

        private async void OnMarcarComoAdotadoClicked(object sender, EventArgs e)
        {
            var botao = sender as Button;
            var petSelecionado = botao?.CommandParameter as Pet;

            if (petSelecionado != null)
            {
                bool confirmar = await DisplayAlert("Confirmação",
                    $"Deseja marcar {petSelecionado.Nome} como adotado? Ele sairá da lista pública de adoção.",
                    "Sim", "Não");

                if (confirmar)
                {
                    bool sucesso = await _database.MarcarComoAdotadoAsync(petSelecionado.IdPet);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", $"{petSelecionado.Nome} encontrou um lar! 🎉", "OK");
                        // Recarrega a lista para sumir com o pet adotado
                        await CarregarMeusPetsAsync();
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Não foi possível atualizar o status do pet.", "OK");
                    }
                }
            }
        }
    }
}