
using Ordering.Api.Extensions;
using Ordering.Infrastructure.Data;

namespace Ordering.Api
{
    public class Startup
    {
        public static async Task Main(string[] args)
        {


            var host = await CreateHostBulder(args).Build()
                        .MigrateDatabase<OrderDbContext>();// Ensure migrations are applied

             host.SeedDatabase<OrderDbContext>( async  (context, services) =>
                {
                    var logger = services.GetRequiredService<ILogger<OrderDbContextSeed>>();
                    await OrderDbContextSeed.SeedAsync(context, logger);
                });

          await  host.RunAsync();

        }

        private static IHostBuilder CreateHostBulder(string[] args)
          => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          });
    }
}
