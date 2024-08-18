using Basket.Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Basket.Api
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
            services.AddControllers();

            services.AddRequiredServices(Configuration);


            //// Add global authorization policies using the DefaultPolicy  == [Authorize]
            ///

            var userPolicy = new AuthorizationPolicyBuilder()
                           .RequireAuthenticatedUser()
                           .Build();

            services.AddControllers(cfg =>
            {
                cfg.Filters.Add(new AuthorizeFilter(userPolicy));
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //  validate jwtbearer token 
                    options.Authority = "https://localhost:9009";
                    options.Audience = "Basket";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Basket.Api v1");
                });

            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //  adding healthcheck 
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            });



        }





    }
}
