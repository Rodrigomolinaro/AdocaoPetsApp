using AdocaoPetsApp.Data;

namespace AdocaoPetsApp.Views;

public partial class CadastroAdotantePage : ContentPage
{
    private readonly DatabaseContext _database;

    public CadastroAdotantePage(DatabaseContext database)
    {
        InitializeComponent();
        _database = database;
    }

    private async void OnSalvarAdotanteClicked(object sender, EventArgs e)
    {
        // Aqui vai aquele bloco de código de validação que lhe enviei anteriormente
        bool sucesso = await _database.CadastrarAdotanteAsync(
            NomeEntry.Text, CpfEntry.Text, TelefoneEntry.Text, EmailEntry.Text,
            CepEntry.Text, EnderecoEntry.Text, CidadeEntry.Text, SenhaEntry.Text
        );

        if (sucesso) await Navigation.PopAsync();
    }
}