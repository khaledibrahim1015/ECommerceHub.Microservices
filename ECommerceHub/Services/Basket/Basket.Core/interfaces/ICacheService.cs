namespace Basket.Core.interfaces;

public interface ICacheService
{
    Task<T> GetDataAsync<T>(string key);
    Task< bool> SetDataAsync<T>(string key , T value , DateTimeOffset expirationTime);
    object RemoveData(string key);

}
