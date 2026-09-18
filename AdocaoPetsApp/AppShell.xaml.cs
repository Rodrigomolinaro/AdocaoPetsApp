using AdocaoPetsApp.Views;

namespace AdocaoPetsApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrando as rotas para a navegação funcionar
            Routing.RegisterRoute("CadastroAdotantePage", typeof(CadastroAdotantePage));
            Routing.RegisterRoute("CadastroOngPage", typeof(CadastroOngPage));
        }
    }
}