using Basket.Infrastructure.Configuration;
using StackExchange.Redis;

namespace Basket.Infrastructure.Helper;



    public class ConnectionHelper
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public ConnectionHelper(AppSettings appSettings)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(appSettings.RedisUrl);
            });
        }

        public ConnectionMultiplexer Connection => _lazyConnection.Value;

    }


