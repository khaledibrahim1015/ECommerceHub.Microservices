using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
namespace Common.Logging;

public static class Logging
{

    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging
        => (hostBuilderContext, loggerConfiguration) =>
        {
            var env = hostBuilderContext.HostingEnvironment;

            loggerConfiguration
                              .MinimumLevel.Information()
                              .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                              .MinimumLevel.Override("System", LogEventLevel.Warning);
            if (env.IsDevelopment())
            {
                var services = new[] { "Catalog", "Basket", "Discount", "Ordering" }; // services namespaces 
                foreach (var service in services)
                    loggerConfiguration.MinimumLevel.Override(service, LogEventLevel.Debug);
            }


            loggerConfiguration
                              .Enrich.FromLogContext()
                              .Enrich.WithExceptionDetails()
                              .Enrich.WithProcessName()
                              .Enrich.WithProcessId()
                              .Enrich.WithThreadName()
                              .Enrich.WithThreadId()
                              .WriteTo.Console();

            // Write To ElasticSearch 
            var elasticUrl = Environment.GetEnvironmentVariable("ElasticUrl")
                                        ?? hostBuilderContext.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = $"ECommerceHub-Logs-{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{env.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                    });
            }





        };


}
