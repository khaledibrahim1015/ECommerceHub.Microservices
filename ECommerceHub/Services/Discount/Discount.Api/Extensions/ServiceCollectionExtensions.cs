using Discount.Application.Interfaces;
using Discount.Application.Queries;
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


            // api registers 
            services.AddMappers();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetDiscountQuery).Assembly));
            services.AddGrpc();



            return services;
        }

        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            var mapperTypes = typeof(IMapper).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(ICommandMapper<,>) ||
                         i.GetGenericTypeDefinition() == typeof(IQueryMapper<,>))));

            foreach (var mapperType in mapperTypes)
            {
                var interfaceType = mapperType.GetInterfaces()
                    .First(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(ICommandMapper<,>) ||
                         i.GetGenericTypeDefinition() == typeof(IQueryMapper<,>)));

                services.AddScoped(interfaceType, mapperType);
            }

            return services;
        }


    }
}
