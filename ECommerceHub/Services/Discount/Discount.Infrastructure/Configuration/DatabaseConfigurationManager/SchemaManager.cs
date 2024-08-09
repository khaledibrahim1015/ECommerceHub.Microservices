using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Discount.Infrastructure.Configuration.DatabaseConfigurationManager;

public  class SchemaManager
{
    private readonly ILogger<SchemaManager> _logger;
    private readonly ConnectionManager _connectionManager;

    public SchemaManager(ConnectionManager connectionManager, ILogger<SchemaManager> logger)
    {
        _connectionManager = connectionManager;
        _logger = logger;
    }

    public async Task CreateSchemaAsync(Type entityType)
    {
        EntityMapper mapper =  new EntityMapper(entityType);

        var columns = mapper.Properties.Select(
            propInfo => $"{mapper.ColumnsMappings[propInfo]}  {GetPostgreSqlType(propInfo)} {GetConstraints(propInfo)}"
            );

        string  createTableSql = $@"
            CREATE TABLE IF NOT EXISTS {mapper.TableName} (
                {string.Join(",\n", columns)}
            )";

        try
        {
            using var  connection = await _connectionManager.GetConnectionAsync() as NpgsqlConnection;
            using var command = connection.CreateCommand();
            command.CommandText = createTableSql;
            await command.ExecuteNonQueryAsync();
            _logger.LogInformation($"Schema created for {entityType.Name}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating schema for {entityType.Name}");
            throw;
        }





    }

    private string GetPostgreSqlType(PropertyInfo property)
    {
        Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => "BOOLEAN",
            TypeCode.Byte => "SMALLINT",
            TypeCode.Int16 => "SMALLINT",
            TypeCode.Int32 => "INTEGER",
            TypeCode.Int64 => "BIGINT",
            TypeCode.Single => "REAL",
            TypeCode.Double => "DOUBLE PRECISION",
            TypeCode.Decimal => "NUMERIC(18,2)",
            TypeCode.DateTime => "TIMESTAMP",
            TypeCode.String => "TEXT",
            _ => throw new NotSupportedException($"Type {type} is not supported.")
        };
    }
    private string GetConstraints(PropertyInfo property)
    {
        StringBuilder constraints = new();

        var primaryKeyAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
        if (primaryKeyAttr != null)
        {
            constraints.Append(" PRIMARY KEY");

            if (primaryKeyAttr.Identity)
            {
                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                {
                    constraints.Append(" GENERATED ALWAYS AS IDENTITY");
                    if (primaryKeyAttr.Seed != 1 || primaryKeyAttr.Increment != 1)
                    {
                        constraints.Append($"({primaryKeyAttr.Seed},{primaryKeyAttr.Increment})");
                    }
                }
                else
                {
                    throw new NotSupportedException("IDENTITY can only be specified on int or long properties.");
                }
            }
        }

        if (property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null)
            constraints.Append(" NOT NULL");

        return constraints.ToString();
    }


}
