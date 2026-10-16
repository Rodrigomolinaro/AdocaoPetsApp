using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;

namespace AdocaoPetsApp.Views
{
    public partial class MinhasAdocoesPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly Usuario _usuarioLogado;

        public MinhasAdocoesPage(DatabaseContext database, Usuario usuarioLogado)
        {
            InitializeComponent();
            _database = database;
            _usuarioLogado = usuarioLogado;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarAdocoes();
        }

        private async Task CarregarAdocoes()
        {
            var minhasAdocoes = await _database.ObterMinhasAdocoesAsync(_usuarioLogado.IdUsuario);
            MinhasAdocoesCollectionView.ItemsSource = minhasAdocoes;
        }
    }
}