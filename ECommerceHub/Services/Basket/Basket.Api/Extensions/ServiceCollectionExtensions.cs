using Basket.Application.Queries;
using Basket.Core.interfaces;
using Basket.Infrastructure.Configuration;
using Basket.Infrastructure.Helper;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basket.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {
            ///  Configure  RedisConfiguration settings  
            services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));


            // Register Packages 
            services.AddMediatR(medCfg =>
            {
                medCfg.RegisterServicesFromAssemblies(typeof(GetBasketByUserNameQuery).Assembly);
            });

            services.AddAutoMapper(typeof(GetBasketByUserNameQuery).GetTypeInfo().Assembly);    
           


            // Register Services 

            services.AddScoped<ICacheService , CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();



            return services;
        }




    }
}
