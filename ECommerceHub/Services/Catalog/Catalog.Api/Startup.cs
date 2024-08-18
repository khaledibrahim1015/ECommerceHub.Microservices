

using Catalog.Api.Factory;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;

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

            //services.AddControllers();

            // Adding All Required Services 
            services.AddRequiredServices(Configuration);


            // Add global authorization policies using the DefaultPolicy  == [Authorize]
            var userPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

            services.AddControllers(cfg =>
            {
                cfg.Filters.Add(new AuthorizeFilter(userPolicy));
            });



            // Add Authentication and Authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        //  validate jwtbearer token 
                        options.Authority = "https://localhost:9009";
                        options.Audience = "Catalog";
                    });
            //  add authorization level 
            // validate token search on scope prop with exist value 
            // [Authorize(Policy ="CanRead")]
            services.AddAuthorization(option =>
            {
                option.AddPolicy("CanRead", configurePolicy =>
                {
                    configurePolicy.RequireClaim("scope", "catalogapi.read");
                });
            });

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
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health",
                   new HealthCheckOptions
                   {
                       Predicate = _ => true,
                       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
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
