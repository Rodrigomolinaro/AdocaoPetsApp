namespace AdocaoPetsApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // APAGUE ou comente qualquer linha que diga MainPage = ...
        // MainPage = new AppShell(); 
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // O aplicativo deve ser iniciado exclusivamente por aqui
        return new Window(new AppShell());
    }
}