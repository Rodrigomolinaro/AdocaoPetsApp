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
                    await DisplayAlert("Sucesso", $"Bem-vindo, {adotante.Nome}!", "OK");
                    // Nas próximas etapas, aqui colocaremos o código para abrir a tela com a Lista de Pets
                }
                else if (usuarioAutenticado is ONG ong)
                {
                    await DisplayAlert("Sucesso", $"Bem-vinda, ONG {ong.Nome}!", "OK");
                    // Nas próximas etapas, aqui colocaremos o código para abrir o Dashboard da ONG para cadastrar os pets
                }
            }
            else
            {
                await DisplayAlert("Erro", "Credenciais incorretas ou usuário não cadastrado.", "OK");
            }
        }

        private async void OnIrParaCadastroAdotanteClicked(object sender, EventArgs e)
        {
            // Navega para a tela de Cadastro de Adotante
            await Shell.Current.GoToAsync("CadastroAdotantePage");
        }

        private async void OnIrParaCadastroOngClicked(object sender, EventArgs e)
        {
            // Navega para a tela de Cadastro de ONG
            await Shell.Current.GoToAsync("CadastroOngPage");
        }
    }
}