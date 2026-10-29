using AdocaoPetsApp.Data;
using AdocaoPetsApp.Views;
using Microsoft.Maui.Controls;

namespace AdocaoPetsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Instancia o contexto do banco de dados e abre o login de forma segura
            var databaseContext = new DatabaseContext();
            MainPage = new NavigationPage(new LoginPage(databaseContext));
        }
    }
}