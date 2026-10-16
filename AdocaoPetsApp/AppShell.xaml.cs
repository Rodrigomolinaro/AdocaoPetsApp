namespace AdocaoPetsApp;

using AdocaoPetsApp.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registo das rotas para as páginas de cadastro
        Routing.RegisterRoute("CadastroAdotantePage", typeof(CadastroAdotantePage));
        Routing.RegisterRoute("CadastroOngPage", typeof(CadastroOngPage));
    }
}