using Basket.Application.DiscountService;
using Basket.Application.Queries;
using Basket.Core.interfaces;
using Basket.Infrastructure.Configuration;
using Basket.Infrastructure.Helper;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Services;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace Basket.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {
            ///  Configure  RedisConfiguration settings  
            services.Configure<RedisConfiguration>(configuration.GetSection("Redis"));
            // Configure  IDistributedCache 
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = configuration.GetValue<string>("Redis:ConnectionString");
            });

            // configure Api Versioning 
            services.AddApiVersioning(apiVersionConfig =>
            {
                apiVersionConfig.DefaultApiVersion = new ApiVersion(1,0);
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

            // Configure RedisHealthcheck !
            services.AddHealthChecks()
                     .AddRedis(configuration.GetValue<string>("Redis:ConnectionString"), "Redis HealthCheck ", HealthStatus.Degraded);


            //  Configure Swagger 
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.Api", Version = "v1" });

            });

            // Register Packages 
            services.AddMediatR(medCfg =>
            {
                medCfg.RegisterServicesFromAssemblies(typeof(GetBasketByUserNameQuery).Assembly);
            });

            services.AddAutoMapper(typeof(GetBasketByUserNameQuery).GetTypeInfo().Assembly);



            // Register Services 
            services.AddScoped<ICacheService , CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<DiscountGrpcService>();
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(configClinet =>
            {
                configClinet.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]);
            });


            return services;
        }




    }
}
