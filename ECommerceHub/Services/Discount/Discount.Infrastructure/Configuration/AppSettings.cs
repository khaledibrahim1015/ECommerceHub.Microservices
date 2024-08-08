using Microsoft.Extensions.Options;

namespace Discount.Infrastructure.Configuration;

public  class AppSettings
{
    private readonly PostgresqlConfiguration _postgresqlConnection;
    public  string ConnectionString
    {
        get 
        { 
            return _postgresqlConnection.ConnectionString;
        }
    }
    public AppSettings(IOptions<PostgresqlConfiguration> options)
    {
        _postgresqlConnection = options.Value;
    }
}
