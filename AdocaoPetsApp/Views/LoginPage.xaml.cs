using AdocaoPetsApp.Models;
using AdocaoPetsApp.Data;

namespace AdocaoPetsApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public LoginPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Verifica se os campos estão em branco
            if (string.IsNullOrWhiteSpace(EmailOuDocumentoEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Preencha seus dados de acesso.", "OK");
                return;
            }

            // Chama o banco de dados para autenticar
            var usuarioAutenticado = await _database.AutenticarAsync(EmailOuDocumentoEntry.Text, SenhaEntry.Text);

            if (usuarioAutenticado != null)
            {
                if (usuarioAutenticado is Usuario adotante)
                {
                    // Abre a tela com a Lista de Pets passando a conexão do banco de dados

                    Application.Current.MainPage = new NavigationPage(new ListagemPetsPage(_database, adotante));
                }
                else if (usuarioAutenticado is ONG ong)
                {
                    // Passamos não apenas o banco, mas também os dados da ONG logada
                    Application.Current.MainPage = new NavigationPage(new DashboardOngPage(_database, ong));
                }
            }
            else
            {
                await DisplayAlert("Erro", "Credenciais incorretas ou usuário não cadastrado.", "OK");
            }
        }

        private async void OnIrParaCadastroAdotanteClicked(object sender, EventArgs e)
        {
            // Navega para a tela de Cadastro de Adotante passando o banco de dados
            await Navigation.PushAsync(new CadastroAdotantePage(_database));
        }

        private async void OnIrParaCadastroOngClicked(object sender, EventArgs e)
        {
            // Navega para a tela de Cadastro de ONG passando o banco de dados
            await Navigation.PushAsync(new CadastroOngPage(_database));
        }
    }
}