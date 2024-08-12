using FluentValidation;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Interfaces;
using Ordering.Application.Queries;
using Ordering.Application.Validators;
namespace Ordering.Api.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(GetOrderListQuery).Assembly);
            });
            services.AddMappers();


            services.AddValidatorsFromAssemblyContaining<CheckoutOrderCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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
