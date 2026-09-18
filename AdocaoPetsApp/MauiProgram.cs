using Microsoft.Extensions.Logging;
using AdocaoPetsApp.Data;
using System.IO;

namespace AdocaoPetsApp
{
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

            // Configura o caminho do banco de dados local
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "AdocaoPets.db3");

            // Registra o DatabaseContext como Singleton
            builder.Services.AddSingleton(s => new DatabaseContext(dbPath));
            builder.Services.AddTransient<Views.CadastroAdotantePage>();
            builder.Services.AddTransient<Views.CadastroOngPage>();
            builder.Services.AddTransient<Views.LoginPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}