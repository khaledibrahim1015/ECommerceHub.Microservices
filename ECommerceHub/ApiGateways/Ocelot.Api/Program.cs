namespace Ocelot.Api;

public class Program
{


    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderCtx, configBuilder) =>
        {
            configBuilder.AddJsonFile($"ocelot.{hostBuilderCtx.HostingEnvironment.EnvironmentName}.json",
                optional: true, reloadOnChange: true);
        })
        .ConfigureWebHostDefaults(webhostbuilder =>
        {
            webhostbuilder.UseStartup<Startup>();
        });


}