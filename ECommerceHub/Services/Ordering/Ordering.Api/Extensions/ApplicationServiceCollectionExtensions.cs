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
            var assembly = typeof(IMapper).Assembly;
            var mapperInterfaces = new[]
            {
                typeof(ICommandMapper<,>),
                typeof(IQueryMapper<,>),
                typeof(IEventCommandMapper<,>)
            };

            var mapperTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.GetInterfaces().Any(i => i.IsGenericType &&
                                mapperInterfaces.Contains(i.GetGenericTypeDefinition())));

            foreach (var mapperType in mapperTypes)
            {
                var interfaces = mapperType.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                mapperInterfaces.Contains(i.GetGenericTypeDefinition()));

                foreach (var interfaceType in interfaces)
                {
                    services.AddScoped(interfaceType, mapperType);
                }
            }

            return services;
        }


    }
}
