﻿using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Discount.Infrastructure.Configuration.DatabaseConfigurationManager;

public class SchemaManager
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
        try
        {
           await EnsureDatabaseAsync();

            // Now proceed with creating the schema in our database
            EntityMapper mapper = new EntityMapper(entityType);

            var columns = mapper.Properties.Select(
                propInfo => $"{mapper.ColumnsMappings[propInfo]} {GetPostgreSqlType(propInfo)} {GetConstraints(propInfo)}"
            );

            string createTableSql = $@"
            CREATE TABLE IF NOT EXISTS {mapper.TableName} (
                {string.Join(",\n", columns)}
            )";

            using var connection = await _connectionManager.GetConnectionAsync() as NpgsqlConnection;
            using var command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE IF EXISTS {mapper.TableName}";
            await command.ExecuteNonQueryAsync();

            command.CommandText = createTableSql;
            await command.ExecuteNonQueryAsync();

            //  JUST INSERT HARD CODED 
            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);";
            command.ExecuteNonQuery();

            _logger.LogInformation($"Schema created for {entityType.Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating schema for {entityType.Name}");
            throw;
        }
    }

    public async Task EnsureDatabaseAsync()
    {
        //  connect to the default 'postgres' database to create our database
        var builder = new NpgsqlConnectionStringBuilder(_connectionManager._connectionString);
        var databaseName = builder.Database;
        builder.Database = "postgres";

        using (var conn = new NpgsqlConnection(builder.ToString()))
        {
            await conn.OpenAsync();

            // Check if our database exists
            using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'", conn))
            {
                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    // Database doesn't exist, so create it
                    using (var createDbCmd = new NpgsqlCommand($"CREATE DATABASE \"{databaseName}\"", conn))
                    {
                        await createDbCmd.ExecuteNonQueryAsync();
                        _logger.LogInformation("Discount Db Migration Completed");

                    }
                }
            }
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
                    constraints.Append(" GENERATED BY DEFAULT AS IDENTITY");
                    if (primaryKeyAttr.Seed != 1 || primaryKeyAttr.Increment != 1)
                    {
                        constraints.Append($" (START WITH {primaryKeyAttr.Seed} INCREMENT BY {primaryKeyAttr.Increment})");
                    }
                }
                else
                {
                    throw new NotSupportedException("IDENTITY can only be specified on int or long properties.");
                }
            }
        }

        if (property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null)
        {
            constraints.Append(" NOT NULL");
        }

        return constraints.ToString();
    }
}
