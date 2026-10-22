using AdocaoPetsApp.Data;
using AdocaoPetsApp.Models;

namespace AdocaoPetsApp.Views
{
    public partial class EditarPetPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly Pet _petAtual;

        // O construtor agora recebe o banco de dados e o pet selecionado
        public EditarPetPage(DatabaseContext database, Pet petParaEditar)
        {
            InitializeComponent();
            _database = database;
            _petAtual = petParaEditar;

            // Preenche os campos assim que a tela abre
            PreencherCampos();
        }

        private void PreencherCampos()
        {
            NomeEntry.Text = _petAtual.Nome;
            EspecieEntry.Text = _petAtual.Especie;
            RacaEntry.Text = _petAtual.Raca;
            IdadeEntry.Text = _petAtual.Idade?.ToString();
            ImagemUrlEntry.Text = _petAtual.ImagemUrl;
            PorteEntry.Text = _petAtual.Porte;
            SexoEntry.Text = _petAtual.Sexo;
            CorEntry.Text = _petAtual.Cor;
            DescricaoEntry.Text = _petAtual.Descricao;
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // --- INÍCIO DA BLINDAGEM DE SEGURANÇA ---
            if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(EspecieEntry.Text))
            {
                await DisplayAlert("Aviso", "O Nome e a Espécie do pet são obrigatórios!", "OK");
                return; // Impede que o código continue e aborte o salvamento
            }
            // --- FIM DA BLINDAGEM ---

            // Sobrescreve os dados do pet já existente na memória
            _petAtual.Nome = NomeEntry.Text;
            _petAtual.Especie = EspecieEntry.Text;
            _petAtual.Raca = RacaEntry.Text;

            // Tratamento seguro para idade (permite que fique vazia/nula)
            int idade = 0;
            if (int.TryParse(IdadeEntry.Text, out idade))
                _petAtual.Idade = idade;
            else
                _petAtual.Idade = null;

            _petAtual.ImagemUrl = ImagemUrlEntry.Text;
            _petAtual.Porte = PorteEntry.Text;
            _petAtual.Sexo = SexoEntry.Text;
            _petAtual.Cor = CorEntry.Text;
            _petAtual.Descricao = DescricaoEntry.Text;

            // Envia a ordem de UPDATE para o Entity Framework sobrescrever no MySQL
            _database.Pets.Update(_petAtual);
            bool sucesso = await _database.SaveChangesAsync() > 0;

            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Dados do pet atualizados com sucesso!", "OK");

                // Fecha a tela de edição e volta automaticamente para a lista de pets da ONG
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", "Ocorreu um erro ao atualizar o pet.", "OK");
            }
        }
    }
}