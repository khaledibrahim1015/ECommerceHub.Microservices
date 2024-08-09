using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Infrastructure.Configuration;
using Discount.Infrastructure.Configuration.DatabaseConfigurationManager;
using Discount.Infrastructure.Repositories;
using Discount.Infrastructure.Services;
namespace Discount.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRequiredServices(this IServiceCollection services , IConfiguration configuration)
        {
            // configure Postgresql configuration 
            services.Configure<PostgresqlConfiguration>(configuration.GetSection("DatabaseSettings"));

            // Register services in DI container 
            services.AddSingleton<AppSettings>();
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<SchemaManager>();

            services.AddSingleton<IEnumerable<Type>>(sp => new List<Type>
            {
                typeof(Coupon),
            });

            services.AddHostedService<DataBaseMigrationService>();

            services.AddScoped<IDiscountRepository, DiscountRepostiory>();

            return services;
        }


    }
}
