using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.Api.Factory
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {

            //adding health check services to container
            //adding MongoDb Health Check
            services.AddHealthChecks()
                         .AddMongoDb(
                        mongodbConnectionString: configuration.GetValue<string>("DatabaseSettings:ConnectionString"),
                        name: "Catalog MongoDb Health Check", failureStatus: HealthStatus.Degraded);

            services.AddSwaggerGen(cw =>
            {
                cw.SwaggerDoc("v1", new OpenApiInfo() { Title = "Catalog.Api", Version = "v1" });
            });

            // Register Mediator 
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            // Register Automapper 
            // handler is in another project
            services.AddAutoMapper(typeof(CreateProductHandler).GetTypeInfo().Assembly);

            services.AddScoped<ICatalogContext , CatalogContext>();
            services.AddScoped<IProductRepostitory, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypeRepository, ProductRepository>();


            return services;
        }
    }
}
