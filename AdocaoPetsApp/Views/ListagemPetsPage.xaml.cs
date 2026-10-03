using AdocaoPetsApp.Services;

namespace AdocaoPetsApp.Views;

public partial class ListagemPetsPage : ContentPage
{
    private readonly PetService _petService;

    public ListagemPetsPage(PetService petService)
    {
        InitializeComponent();
        _petService = petService;
    }

    // Este método roda automaticamente sempre que a página aparece na tela
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarPets();
    }

    private async Task CarregarPets()
    {
        try
        {
            // Busca a lista do MySQL
            var pets = await _petService.GetPetsAsync();

            // Preenche a CollectionView com os dados
            PetsCollectionView.ItemsSource = pets;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao carregar os dados: {ex.Message}", "OK");
        }
    }
}