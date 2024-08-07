using Basket.Core.Entities;

namespace Basket.Core.interfaces;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName);
    Task<ShoppingCart> UpdateBasket(ShoppingCart cart);
    Task DeleteBasket (string userName);

}
