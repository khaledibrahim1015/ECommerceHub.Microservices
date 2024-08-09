using Discount.Infrastructure.Configuration.DatabaseConfigurationManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Discount.Infrastructure.Services;

public class DataBaseMigrationService : IHostedService
{
    private readonly SchemaManager _schemaManager;
    private readonly ILogger<DataBaseMigrationService> _logger;
    private readonly IEnumerable<Type> _entityTypes;

 

    public DataBaseMigrationService(SchemaManager schemaManager,
                            ILogger<DataBaseMigrationService> logger,
                            IEnumerable<Type> entityTypes)
    {
        _schemaManager = schemaManager;
        _logger = logger;
        _entityTypes = entityTypes;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await MigrateDatabaseAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
          => Task.CompletedTask;


    private async Task MigrateDatabaseAsync(CancellationToken cancellationToken)
    {
        foreach (var entityType in _entityTypes)
        {

            try
            {
              await  _schemaManager.CreateSchemaAsync(entityType);
                _logger.LogInformation($"Migrate succesed for {entityType.Name}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error migrating database for {entityType.Name}");
            }




        }




    }

}
