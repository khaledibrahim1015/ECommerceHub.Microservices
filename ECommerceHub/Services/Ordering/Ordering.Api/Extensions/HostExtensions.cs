using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
namespace Ordering.Api.Extensions;

public static class HostExtensions
{
    public static async Task<IHost> MigrateDatabase<TContext>(this IHost host)
        where TContext : DbContext
    {
        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation($"Starting database migration for context {typeof(TContext).Name}");

            var retryPolicy = CreateRetryPolicy(logger);
            await retryPolicy.Execute(async () =>
            {

                // Create database if it doesn't exist
                var isExist = await context.Database.EnsureCreatedAsync();
                if (isExist)
                    logger.LogInformation($"{isExist}  Database is created ");
                else
                    logger.LogInformation($"{isExist} Database already exist ");
                // Apply migrations
                if (context.Database.GetPendingMigrations().Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await context.Database.MigrateAsync();
                }
                else
                {
                    logger.LogInformation("No pending migrations.");
                }


            }
         );
            logger.LogInformation($"Database migration completed for context {typeof(TContext).Name}");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while migrating the database for context {typeof(TContext).Name}");
            throw;  // Re-throw the exception to avoid silent failures.

        }
        return host;
    }

    public static IHost SeedDatabase<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TContext>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation($"Starting database seeding for context {typeof(TContext).Name}");
            var retryPolicy = CreateRetryPolicy(logger);

            retryPolicy.Execute(() => seeder.Invoke(context, services).GetAwaiter().GetResult());
            logger.LogInformation($"Database seeding completed for context {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {

            logger.LogError(ex, $"An error occurred while seeding the database for context {typeof(TContext).Name}");
            throw;
        }



        return host;
    }


    private static RetryPolicy CreateRetryPolicy(ILogger logger)
    {

        var retryPloicy = Policy.Handle<SqlException>().Or<TimeoutException>()
                            .WaitAndRetry(
                                retryCount: 5,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (exception, timeSpan, retryCount, context) =>
                                {
                                    logger.LogWarning($"Retry {retryCount} due to: {exception.Message}. Waiting {timeSpan} before next attempt.");
                                }
                                );
        return retryPloicy;
    }


}
