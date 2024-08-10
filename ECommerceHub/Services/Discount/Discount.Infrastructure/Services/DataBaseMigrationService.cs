using Discount.Infrastructure.Configuration.DatabaseConfigurationManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Services;

public class DataBaseMigrationService : IHostedService
{
    private readonly SchemaManager _schemaManager;
    private readonly ILogger<DataBaseMigrationService> _logger;
    private readonly IEnumerable<Type> _entityTypes;
    private readonly ConnectionManager _connectionManager;

    public DataBaseMigrationService(SchemaManager schemaManager,
                                    ILogger<DataBaseMigrationService> logger,
                                    IEnumerable<Type> entityTypes,
                                    ConnectionManager connectionManager)
    {
        _schemaManager = schemaManager;
        _logger = logger;
        _entityTypes = entityTypes;
        _connectionManager = connectionManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await MigrateDatabaseAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task MigrateDatabaseAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Discount Db Migration started");
        try
        {
            await _schemaManager.EnsureDatabaseAsync();

            using var connection = await _connectionManager.GetConnectionAsync() as NpgsqlConnection;
            if (connection != null)
            {

                foreach (var entityType in _entityTypes)
                {
                    try
                    {
                        await _schemaManager.CreateSchemaAsync(entityType);
                        _logger.LogInformation($"Migrate succeeded for {entityType.Name}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error migrating database for {entityType.Name}");
                    }
                }
            }
            else
            {
                _logger.LogError("Failed to establish a connection to the database.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ensuring database exists");
        }

    }

}
