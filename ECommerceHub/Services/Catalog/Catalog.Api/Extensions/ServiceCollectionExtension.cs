﻿using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging.Correlation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.Api.Factory
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRequiredServices(this IServiceCollection services, IConfiguration configuration)
        {


            //  services.AddApiVersioning():
            //  This method adds the core API versioning services to your application.It configures how API versions are interpreted and applied.
            //            options.DefaultApiVersion = new ApiVersion(1, 0): Sets the default API version to 1.0.
            //  options.AssumeDefaultVersionWhenUnspecified = true: If a client doesn't specify a version, the default version will be used.
            //  options.ReportApiVersions = true: Adds API version information to response headers.


            //  services.AddVersionedApiExplorer():
            //  This method adds and configures API explorer services that are version - aware.It's particularly useful for generating documentation (like Swagger) for different API versions.

            //  options.GroupNameFormat = "'v'VVV": Defines the format of the version name in the URL. 'v'VVV would result in something like 'v1', 'v2', etc.
            //  options.SubstituteApiVersionInUrl = true: Allows the version to be specified as a URL segment, like / api / v1 / controller.

            //Key differences:
            //Purpose:
            //        AddApiVersioning: Core versioning functionality.
            //        AddVersionedApiExplorer: Enhances API exploration and documentation for versioned APIs.



            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader("api-version")
                    );
            }).AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;

            });



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
            // handler is in another project
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies((typeof(GetAllProductsHandler).Assembly));
            });

            // Register Automapper 

            services.AddAutoMapper(typeof(CreateProductHandler).GetTypeInfo().Assembly);

            services.AddScoped<ICatalogContext>(sp => new CatalogContext(sp.GetRequiredService<IConfiguration>()));
            services.AddScoped<IProductRepostitory, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypeRepository, ProductRepository>();
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }
    }
}
