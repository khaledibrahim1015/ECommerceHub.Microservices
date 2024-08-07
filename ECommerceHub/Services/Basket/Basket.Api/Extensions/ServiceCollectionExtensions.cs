using Basket.Core.interfaces;
using Basket.Infrastructure.Configuration;
using Basket.Infrastructure.Helper;
using Basket.Infrastructure.Services;

namespace Basket.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {
            ///  Configure  RedisConfiguration settings  
            services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));




            // Register Services 

            services.AddScoped<ICacheService , CacheService>();




            return services;
        }




    }
}
