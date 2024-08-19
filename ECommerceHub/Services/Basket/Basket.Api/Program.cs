using Common.Logging;
using Serilog;
namespace Basket.Api
{
    public class Program
    {

        public async static Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
         => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webHostBuilder =>
         {
             webHostBuilder.UseStartup<Startup>();
         }).ConfigureAppConfiguration((hostingContext, configBuilder) =>
         {
             configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
             configBuilder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
         }).UseSerilog(Logging.ConfigureLogging);
    }
}
