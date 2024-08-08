using Discount.Core.Repositories;
using Discount.Infrastructure.Configuration;
using Discount.Infrastructure.Repositories;

namespace Discount.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static ServiceCollection AddRequiredServices(this ServiceCollection services , IConfiguration configuration)
        {
            // configure Postgresql configuration 
            services.Configure<PostgresqlConfiguration>(configuration.GetSection("DatabaseSettings"));

            // Register services in DI container 
            services.AddSingleton<AppSettings>();
            services.AddSingleton<ConnectionManager>();

            services.AddScoped<IDiscountRepository, DiscountRepostiory>();

            return services;
        }


    }
}
