using AdocaoPetsApp.Models;
using AdocaoPetsApp.Data;

namespace AdocaoPetsApp.Views
{
    public partial class CadastroAdotantePage : ContentPage
    {
        private readonly DatabaseContext _database;

        // Injetando o contexto do banco de dados no construtor
        public CadastroAdotantePage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(CpfEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha os campos obrigatórios.", "OK");
                return;
            }

            // Cria o objeto Usuario com os dados da tela
            var novoUsuario = new Usuario
            {
                Nome = NomeEntry.Text,
                CPF = CpfEntry.Text,
                Email = EmailEntry.Text,
                Telefone = TelefoneEntry.Text,
                Cidade = CidadeEntry.Text,
                Senha = SenhaEntry.Text
            };

            // Salva no banco de dados local
            await _database.SalvarUsuarioAsync(novoUsuario);

            await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");

            // Limpa os campos após o cadastro
            NomeEntry.Text = string.Empty;
            CpfEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            TelefoneEntry.Text = string.Empty;
            CidadeEntry.Text = string.Empty;
            SenhaEntry.Text = string.Empty;

            // Aqui você pode redirecionar para a tela de Login (história 1.2.3)
        }
    }
}