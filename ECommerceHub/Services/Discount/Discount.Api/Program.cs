
using Common.Logging;
using Serilog;

namespace Discount.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webHostBuilder =>
        {
            webHostBuilder.UseStartup<Startup>();
        }).UseSerilog(Logging.ConfigureLogging);
    }
}
