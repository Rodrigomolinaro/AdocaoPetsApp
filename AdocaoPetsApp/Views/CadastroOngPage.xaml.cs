using AdocaoPetsApp.Models;
using AdocaoPetsApp.Data;

namespace AdocaoPetsApp.Views
{
    public partial class CadastroOngPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public CadastroOngPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        private async void OnCadastrarOngClicked(object sender, EventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(NomeOngEntry.Text) || string.IsNullOrWhiteSpace(CnpjEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha os campos obrigatórios (Nome, CNPJ e Senha).", "OK");
                return;
            }

            // Cria o objeto ONG com os dados da tela
            var novaOng = new ONG
            {
                Nome = NomeOngEntry.Text,
                CNPJ = CnpjEntry.Text,
                Email = EmailEntry.Text,
                Telefone = TelefoneEntry.Text,
                Cidade = CidadeEntry.Text,
                Senha = SenhaEntry.Text
            };

            // Salva no banco de dados local
            await _database.SalvarOngAsync(novaOng);

            await DisplayAlert("Sucesso", "ONG cadastrada com sucesso! Pronta para salvar vidas.", "OK");

            // Limpa os campos
            NomeOngEntry.Text = string.Empty;
            CnpjEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            TelefoneEntry.Text = string.Empty;
            CidadeEntry.Text = string.Empty;
            SenhaEntry.Text = string.Empty;
        }
    }
}