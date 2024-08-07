using Microsoft.Extensions.Options;

namespace Basket.Infrastructure.Configuration;

public  class AppSettings
{
    public RedisConfiguration RedisConfiguration { get; }

    public string RedisUrl => RedisConfiguration.RedisUrl;

    public AppSettings(IOptions<RedisConfiguration> options)
    {
        RedisConfiguration = options.Value;
    }


}
