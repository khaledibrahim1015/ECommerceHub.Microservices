using Ordering.Core.interfaces;
using Ordering.Infrastructure.Configuration;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Api.Extensions
{
    public static class InfraServiceCollectionExtensions
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("DatabaseSettings"));
            services.AddSingleton<OrderDbContextOptionFactory>();
            services.AddScoped(serviceProvider =>
            {
                var dbcontextFactory = serviceProvider.GetRequiredService<OrderDbContextOptionFactory>();
                return new OrderDbContext(dbcontextFactory.CreateOrderDbContextOptions());
            });


            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;

        }


    }
}
