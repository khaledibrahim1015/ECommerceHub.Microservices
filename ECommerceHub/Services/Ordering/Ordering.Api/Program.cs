using Common.Logging;
using Ordering.Api.Extensions;
using Ordering.Infrastructure.Data;
using Serilog;

namespace Ordering.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var host = await CreateHostBulder(args).Build()
                        .MigrateDatabase<OrderDbContext>();// Ensure migrations are applied

            host.SeedDatabase<OrderDbContext>(ExecuteDatabaseSeedAsync);

            await host.RunAsync();

        }

        private static IHostBuilder CreateHostBulder(string[] args)
          => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          }).ConfigureAppConfiguration((hostBuilderContext, configurationBuiler) =>
          {
              configurationBuiler.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
              configurationBuiler.AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
          }).UseSerilog(Logging.ConfigureLogging);
        //  .ConfigureLogging((hostingContext, logging) =>
        //{
        //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //    logging.AddConsole();
        //    logging.AddDebug();
        //})


        private static async Task ExecuteDatabaseSeedAsync(OrderDbContext context, IServiceProvider service)
        {
            var logger = service.GetRequiredService<ILogger<OrderDbContextSeed>>();
            await OrderDbContextSeed.SeedAsync(context, logger);
        }

    }
}
