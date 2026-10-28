using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Data;

// O namespace é fundamental para as outras pastas (Android, iOS, Windows) acharem este arquivo!
namespace AdocaoPetsApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 1. A sua connection string (porta 3307 do TCC)
        string connectionString = "Server=mysql-rodrigomolinaro.alwaysdata.net;Port=3306;Database=rodrigomolinaro_pets;Uid=rodrigomolinaro_pets;Pwd=Rm541632;";

        // 2. Configuração do Entity Framework com o Pomelo MySQL
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        // 3. Registramos as páginas para a injeção de dependência funcionar corretamente
        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.CadastroAdotantePage>();
        builder.Services.AddTransient<Views.CadastroOngPage>();
        builder.Services.AddTransient<Views.ListagemPetsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}