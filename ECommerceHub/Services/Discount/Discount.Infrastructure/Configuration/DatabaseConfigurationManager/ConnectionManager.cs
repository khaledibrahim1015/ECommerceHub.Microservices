using Npgsql;
using System.Data;
using System.Data.Common;

namespace Discount.Infrastructure.Configuration.DatabaseConfigurationManager
{
    public class ConnectionManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly object _lock = new object();
        private DbConnection _dbConnection;

        public ConnectionManager(AppSettings appSettings)
        {
            _connectionString = appSettings.ConnectionString;

        }

        public async Task<DbConnection> GetConnectionAsync()
        {

            if (_dbConnection == null)
            {
                lock (_lock)
                {
                    if (_dbConnection == null)
                    {
                        _dbConnection = new NpgsqlConnection(_connectionString);
                    }
                }
            }

            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }

            return _dbConnection;
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
                _dbConnection = null;
            }
        }
    }
}
