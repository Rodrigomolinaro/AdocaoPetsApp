using AdocaoPetsApp.Models;
using AdocaoPetsApp.Services;

namespace AdocaoPetsApp.Views;

public partial class CadastroPetPage : ContentPage
{
    private readonly PetService _petService;

    // O MAUI injeta automaticamente o PetService aqui
    public CadastroPetPage(PetService petService)
    {
        InitializeComponent();
        _petService = petService;
    }
    private async void OnVerPetsClicked(object sender, EventArgs e)
    {
        // Navega para a tela de listagem usando a rota que registámos
        await Shell.Current.GoToAsync(nameof(ListagemPetsPage));
    }
    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        // Validação básica
        if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(EspecieEntry.Text))
        {
            await DisplayAlert("Erro", "Preencha pelo menos o nome e a espécie.", "OK");
            return;
        }

        // Instancia o modelo com os dados digitados
        var novoPet = new Pet
        {
            Nome = NomeEntry.Text,
            Especie = EspecieEntry.Text,
            Raca = RacaEntry.Text,
            // Adapta esta propriedade de acordo com o teu model Pet.cs
            // Se a idade for int, usamos o int.TryParse
            // Idade = int.TryParse(IdadeEntry.Text, out int idade) ? idade : 0
        };

        try
        {
            // Envia para o serviço gravar no MySQL
            await _petService.SalvarPetAsync(novoPet);

            await DisplayAlert("Sucesso", $"{novoPet.Nome} foi salvo no banco de dados!", "OK");

            // Limpa os campos após salvar
            NomeEntry.Text = "";
            EspecieEntry.Text = "";
            RacaEntry.Text = "";
            IdadeEntry.Text = "";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao salvar", ex.Message, "OK");
        }
    }
}