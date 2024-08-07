using Basket.Core.interfaces;
using Basket.Infrastructure.Helper;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _db;

    public CacheService()
    {
        _db  =  ConnectionHelper.Connection.GetDatabase();
    }

    public async Task< T?> GetDataAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (!string.IsNullOrEmpty(value))
             return JsonConvert.DeserializeObject<T>(value);

        return default; 
    }


    public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

        bool isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
        return isSet;

    }
    public object RemoveData(string key)
    {
        bool _isKeyExist = _db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return _db.KeyDelete(key);
        }
        return false;

    }

}
