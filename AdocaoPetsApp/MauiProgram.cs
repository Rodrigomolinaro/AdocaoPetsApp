using AdocaoPetsApp.Data;
using AdocaoPetsApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

            // Configuração do banco de dados MySQL
            string connectionString = "Server=localhost;Port=3307;Database=tcc_pets;Uid=root;Pwd=;";
            // Lembra-te: Usa Server=10.0.2.2 se fores testar no emulador do Android!

            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddTransient<PetService>();
            builder.Services.AddTransient<Views.CadastroPetPage>();
            builder.Services.AddTransient<Views.ListagemPetsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // A LINHA QUE ESTAVA A FALTAR:
            return builder.Build();
        }
    }
}