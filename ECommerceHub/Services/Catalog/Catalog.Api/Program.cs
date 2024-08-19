
using Catalog.Infrastructure.Data;
using Common.Logging;
using Serilog;

namespace Catalog.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();//.Run();

            using IServiceScope? scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            IHostEnvironment env = services.GetRequiredService<IHostEnvironment>();
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                if (env.IsDevelopment() || env.IsStaging())
                {
                    ICatalogContext context = services.GetRequiredService<ICatalogContext>();
                    logger.LogInformation("Finished seeding the database.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            }).ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                configBuilder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }).UseSerilog(Logging.ConfigureLogging);

    }
}
