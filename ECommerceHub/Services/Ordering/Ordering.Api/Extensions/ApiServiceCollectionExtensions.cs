using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Ordering.Api.EventBusConsumer;
using Ordering.Infrastructure.Data;

namespace Ordering.Api.Extensions
{
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // configure Api Versioning 
            services.AddApiVersioning(apiVersionConfig =>
            {
                apiVersionConfig.DefaultApiVersion = new ApiVersion(1, 0);
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

            //// Configure RedisHealthcheck !
            //services.AddHealthChecks()
            //    .AddSqlServer(connectionString: configuration["DatabaseSettings:ConnectionString"],
            //      name: "SqlServer HealthCheck ", failureStatus: HealthStatus.Degraded);

            services.AddHealthChecks().Services.AddDbContext<OrderDbContext>();

            //  Configure Swagger 
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ordering.Api", Version = "v1" });

            });




            /// Configure RabbitMq Using MassTransit 
            //  For Consumer Event 
            // Configure RabbitMQ for Consuming Messages
            services.AddScoped<BasketOrderingConsumer>();
            //services.AddMassTransit(config =>
            //{
            //    config.SetKebabCaseEndpointNameFormatter();
            //    config.AddConsumer<BasketOrderingConsumer>();
            //    config.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        var rabbitMqHostAddress = Environment.GetEnvironmentVariable("HostAddress") ?? configuration.GetValue<string>("EventBusSettings:HostAddress");
            //        cfg.Host(new Uri(rabbitMqHostAddress), host =>
            //        {
            //            var userName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? configuration.GetValue<string>("EventBusSettings:UserName");
            //            var pass = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? configuration.GetValue<string>("EventBusSettings:Password");
            //            host.Username(userName);
            //            host.Password(pass);
            //        });

            //        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, e =>
            //        {
            //            e.ConfigureConsumeTopology = true;
            //            e.Bind(EventBusConstant.BasketOrderExchange, x =>
            //            {
            //                x.ExchangeType = ExchangeType.Direct;
            //                x.RoutingKey = EventBusConstant.BasketCheckoutQueue;
            //            });
            //            e.ConfigureConsumer<BasketOrderingConsumer>(ctx);
            //        });

            //        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            //    });
            //});

            //services.AddMassTransitHostedService(); // Ensure MassTransit runs as a hosted service

            services.AddMassTransit(config =>
            {
                //Mark this as consumer
                config.AddConsumer<BasketOrderingConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMqHostAddress = Environment.GetEnvironmentVariable("EventBusSettings") ?? configuration.GetValue<string>("EventBusSettings:HostAddress");
                    cfg.Host(rabbitMqHostAddress);
                    //provide the queue name with consumer settings
                    cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }

    }
}
