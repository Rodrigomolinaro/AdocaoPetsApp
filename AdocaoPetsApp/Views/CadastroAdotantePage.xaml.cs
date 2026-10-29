using AdocaoPetsApp.Data;
using Microsoft.Maui.Controls;
using System;

namespace AdocaoPetsApp.Views
{
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
            try
            {
                // Recolhe os dados dos campos baseando-se na estrutura do seu banco rodrigomolinaro_pets
                string nome = NomeEntry?.Text ?? string.Empty;
                string cpf = CpfEntry?.Text ?? string.Empty;
                string email = EmailEntry?.Text ?? string.Empty;
                string senha = SenhaEntry?.Text ?? string.Empty;

                // Validação básica de campos obrigatórios
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
                    return;
                }

                // Se houver um DatePicker no seu XAML com outro nome, ajuste aqui. 
                // Caso contrário, definimos a data atual como padrão temporário para evitar falhas:
                DateTime? dataNascimento = DateTime.Now;

                bool sucesso = await _database.CadastrarAdotanteAsync(
    NomeEntry.Text,
    TelefoneEntry.Text,
    EmailEntry.Text,
    EnderecoEntry.Text,
    CidadeEntry.Text,
    CepEntry.Text,
    CpfEntry.Text,
    SenhaEntry.Text
);

                if (sucesso)
                {
                    await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível realizar o cadastro no banco de dados.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro Crítico", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }
    }
}