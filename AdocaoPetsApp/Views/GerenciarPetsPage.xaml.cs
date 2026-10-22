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
            // O seu método já está pronto no DatabaseContext! Ele puxa só os pets desta ONG
            var meusPets = await _database.ObterPetsPorOngAsync(_ongLogada.IdOng);
            PetsCollectionView.ItemsSource = meusPets;
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