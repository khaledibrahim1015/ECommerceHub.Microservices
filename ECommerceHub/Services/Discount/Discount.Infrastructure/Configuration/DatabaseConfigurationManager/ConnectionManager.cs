using Npgsql;
using System.Data;
using System.Data.Common;

namespace Discount.Infrastructure.Configuration.DatabaseConfigurationManager
{
    public class ConnectionManager
    {
        public  readonly string _connectionString;

        public ConnectionManager(AppSettings appSettings)
        {
            _connectionString = appSettings.ConnectionString;
        }

        public async Task<DbConnection> GetConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
