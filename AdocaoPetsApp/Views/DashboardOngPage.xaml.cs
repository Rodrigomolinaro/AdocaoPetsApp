using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models; // Necessário para reconhecer a classe ONG

namespace AdocaoPetsApp.Views
{
    public partial class DashboardOngPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly ONG _ongLogada; // Guarda a ONG que está usando o app

        // O construtor agora recebe a ONG
        public DashboardOngPage(DatabaseContext database, ONG ongLogada)
        {
            InitializeComponent();
            _database = database;
            _ongLogada = ongLogada;
        }

        private async void OnCadastrarPetClicked(object sender, EventArgs e)
        {
            // Repassa o banco de dados e a ONG para a tela de cadastro
            await Navigation.PushAsync(new CadastroPetPage(_database, _ongLogada));
        }

        private async void OnGerenciarPetsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GerenciarPetsPage(_database, _ongLogada));
        }
        private void OnSairClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage(_database));
        }
        private async void OnVerPedidosClicked(object sender, EventArgs e)
        {
            // Navega para a tela de aprovação de adoções
            await Navigation.PushAsync(new SolicitacoesPage(_database));
        }
    }
}