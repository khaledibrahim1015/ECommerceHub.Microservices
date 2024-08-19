using Common.Logging.Correlation;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot.Api;

public class Startup
{

    public IConfiguration Configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddOcelot()
                 .AddCacheManager(o => o.WithDictionaryHandle());



    }
    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();



        app.UseRouting();
        app.AddCorrelationIdMiddleware();
        app.UseEndpoints(endpoints =>
        {

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("hello from ocelot");
            });
        });

        await app.UseOcelot();

    }






}
