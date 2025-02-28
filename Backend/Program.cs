using Backend.Configuring;
using Backend.Data;
using Backend.Tests.Cryptography;
using Backend.Tests.Serialization;
using Backend.Tests.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Backend {
    public class Program {
        private static string GetAppUrl(IConfiguration configuration) {
            var protocol = configuration["AppUrl:Protocol"] ?? "INVALID";
            var host     = configuration["AppUrl:Host"    ] ?? "INVALID";
            var port     = configuration["AppUrl:Port"    ] ?? "INVALID";
            var url = $"{protocol}://{host}:{port}";
            return url;
        }
        private static void SetAppContext(WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<ApplicationContext>();
                    context.Database.Migrate();
                    DbInitializer.Initialize(context);
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, Constants.ErrMsgs.DB_INIT_ERROR_OCCURED);
                }
            }
        }
        public static void Main(string[] args) {
            CryptographyTests .Test();
            SerializationTests.Test();
            ValidationTests   .Test();

            var builder = WebApplication.CreateBuilder(args);
            ServicesConfigurator.Configure(
                services      : builder.Services     ,
                configuration : builder.Configuration
            );
            var app = builder.Build();
            SetAppContext(app);

            bool isDevelopmentEnv = app.Environment.IsDevelopment();
            AppConfigurator.Configure(
                app                                 , 
                configuration   : app.Configuration , 
                isDevelopmentEnv: true
            );

            var appUrl = GetAppUrl(app.Configuration);
            app.Urls.Add(appUrl);
            app.Run();
        }
    }
}
