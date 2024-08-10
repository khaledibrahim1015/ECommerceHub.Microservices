using Ordering.Core.Entities;

namespace Ordering.Core.interfaces;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}
