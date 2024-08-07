using Basket.Core.Entities;
using Basket.Core.interfaces;
using Basket.Infrastructure.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async  Task<ShoppingCart> GetBasket(string userName)
    {
       var basket  = await _redisCache.GetStringAsync(userName);
      if(!string.IsNullOrEmpty(basket)) 
        return JsonConvert.DeserializeObject<ShoppingCart>(basket)!;

      return default(ShoppingCart)!;
    }

    public async Task DeleteBasket(string userName)
    {
      await _redisCache.RemoveAsync(userName);
    }

 

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        await  _redisCache.SetStringAsync(shoppingCart.UserName , JsonConvert.SerializeObject(shoppingCart));
        return  await GetBasket(shoppingCart.UserName);
    }
}
