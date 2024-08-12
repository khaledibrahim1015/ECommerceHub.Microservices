using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Ordering.Infrastructure.Data;

namespace Ordering.Api.Extensions
{
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // configure Api Versioning 
            services.AddApiVersioning(apiVersionConfig =>
            {
                apiVersionConfig.DefaultApiVersion = new ApiVersion(1, 0);
                apiVersionConfig.AssumeDefaultVersionWhenUnspecified = true;
                apiVersionConfig.ReportApiVersions = true;
                apiVersionConfig.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader("api-version")
                    );
            }).AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;

            });

            //// Configure RedisHealthcheck !
            //services.AddHealthChecks()
            //    .AddSqlServer(connectionString: configuration["DatabaseSettings:ConnectionString"],
            //      name: "SqlServer HealthCheck ", failureStatus: HealthStatus.Degraded);

            services.AddHealthChecks().Services.AddDbContext<OrderDbContext>();

            //  Configure Swagger 
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ordering.Api", Version = "v1" });

            });


            return services;
        }

    }
}
