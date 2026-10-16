namespace AdocaoPetsApp;

using AdocaoPetsApp.Views;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        // O NavigationPage habilita a navegação entre as telas e cria a barra superior nativa
        MainPage = new NavigationPage(loginPage);
    }
}