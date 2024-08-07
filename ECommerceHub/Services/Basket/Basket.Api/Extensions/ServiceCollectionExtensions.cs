using Basket.Infrastructure.Configuration;
using Basket.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;

namespace Basket.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {
            ///  Register RedisConfiguration 
            services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));
            // Configure Redis settings
            services.AddSingleton<AppSettings>();
            // Register ConnectionHelper
            services.AddSingleton<ConnectionHelper>();






            return services;
        }




    }
}
