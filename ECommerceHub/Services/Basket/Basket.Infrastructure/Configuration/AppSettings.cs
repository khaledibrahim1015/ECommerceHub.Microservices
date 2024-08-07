using Microsoft.Extensions.Options;

namespace Basket.Infrastructure.Configuration;

public  class AppSettings
{
    public static RedisConfiguration RedisConfiguration { get; private set; }

    public static string RedisUrl => RedisConfiguration.RedisUrl;

    public AppSettings(IOptions<RedisConfiguration> options)
    {
        RedisConfiguration = options.Value;
    }


}
