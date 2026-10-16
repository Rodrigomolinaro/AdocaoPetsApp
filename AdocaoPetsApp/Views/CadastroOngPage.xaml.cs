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
                Cnpj = CnpjEntry.Text,
                Email = EmailEntry.Text,
                Telefone = TelefoneEntry.Text,
                Cidade = CidadeEntry.Text,
                Senha = SenhaEntry.Text
            };

            // Salva no banco de dados local e guarda o resultado
            bool sucesso = await _database.SalvarOngAsync(novaOng);

            if (sucesso)
            {
                await DisplayAlert("Sucesso", "ONG cadastrada com sucesso! Pronta para salvar vidas.", "OK");
                // Retorna automaticamente para a tela de Login
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", "Ocorreu um problema ao salvar os dados da ONG no banco de dados.", "OK");
            }
        }
    }
}