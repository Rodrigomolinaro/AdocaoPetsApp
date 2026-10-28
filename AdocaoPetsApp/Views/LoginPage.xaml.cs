using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Models;
using AdocaoPetsApp.Data;
using Microsoft.Maui.Controls;
using AdocaoPetsApp.Views;

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
            try
            {
                string login = EmailOuDocumentoEntry.Text?.Trim();
                string senha = SenhaEntry.Text?.Trim();

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
                {
                    await DisplayAlert("Aviso", "Por favor, preencha todos os campos.", "OK");
                    return;
                }

                // 1. TENTA ENCONTRAR COMO ADOTANTE
                var usuarioLogado = await _database.Usuarios
                    .FirstOrDefaultAsync(u => (u.Email == login || u.CPF == login) && u.Senha == senha);

                if (usuarioLogado != null)
                {
                    // Correção: Redirecionando para a ListagemPetsPage existente no projeto
                    await Navigation.PushAsync(new ListagemPetsPage(_database, usuarioLogado));
                    return;
                }

                // 2. TENTA ENCONTRAR COMO ONG
                // Nota: Verifique se no seu DatabaseContext está escrito "ONGs" ou "Ongs"
                var ongLogada = await _database.ONGs
                    .FirstOrDefaultAsync(o => (o.Email == login || o.Cnpj == login) && o.Senha == senha);

                if (ongLogada != null)
                {
                    await Navigation.PushAsync(new DashboardOngPage(_database, ongLogada));
                    return;
                }

                // 3. SE NÃO ACHOU EM NENHUMA DAS DUAS
                await DisplayAlert("Erro", "Login ou senha incorretos. Verifique os dados e tente novamente.", "OK");
            }
            catch (Exception ex)
            {
                string erroReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await DisplayAlert("Erro Crítico", $"{erroReal}\n\nRastreio:\n{ex.StackTrace}", "OK");
            }
        }

        private async void OnIrParaCadastroAdotanteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroAdotantePage(_database));
        }

        private async void OnIrParaCadastroOngClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroOngPage(_database));
        }
    }
}