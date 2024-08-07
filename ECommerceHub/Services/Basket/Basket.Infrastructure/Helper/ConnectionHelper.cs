using Basket.Infrastructure.Configuration;
using StackExchange.Redis;

namespace Basket.Infrastructure.Helper;

    public class ConnectionHelper
    {
        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        static ConnectionHelper()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(AppSettings.RedisUrl);
            });
        }

        public static  ConnectionMultiplexer Connection => _lazyConnection.Value;

    }


