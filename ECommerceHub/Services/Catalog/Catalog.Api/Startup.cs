

using Catalog.Api.Factory;
using Catalog.Application.Handlers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Api
{
    public class Startup
    {

        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();

            // Adding All Required Services 
            services.AddRequiredServices(Configuration);



        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(sw =>
                {
                    sw.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Catalog.Api v1");
                });
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health",
                   new HealthCheckOptions
                   {
                       Predicate = _ => true,
                       ResponseWriter =  UIResponseWriter.WriteHealthCheckUIResponse,
                       //ResponseWriter = async (context, report) =>
                       //{   
                       //    var result = JsonSerializer.Serialize(
                       //        new
                       //        {
                       //            status = report.Status.ToString(),
                       //            errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                       //        });

                       //    context.Response.ContentType = MediaTypeNames.Application.Json;

                       //    await context.Response.WriteAsync(result);
                       //}
                   });
            });



        }


    }
}
