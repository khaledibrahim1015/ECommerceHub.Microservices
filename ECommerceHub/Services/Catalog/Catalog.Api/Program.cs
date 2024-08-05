
using Catalog.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host =  CreateHostBuilder(args).Build();//.Run();

            using IServiceScope? scope =  host.Services.CreateScope();
            IServiceProvider services  =  scope.ServiceProvider;
            ICatalogContext context   =  services.GetRequiredService<ICatalogContext>();
            IHostEnvironment env  =  services.GetRequiredService<IHostEnvironment>();
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                if(env.IsDevelopment() || env.IsStaging())
                {
                    // seed data 
                    GenericContextSeed.SeedData(context.Prodcuts);
                    GenericContextSeed.SeedData(context.Brands);
                    GenericContextSeed.SeedData(context.Types);
                    logger.LogInformation("Finished seeding the database.");
                }
            }
            catch (Exception ex )
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }

            host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            }).ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                configBuilder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            });
        
    }
}
