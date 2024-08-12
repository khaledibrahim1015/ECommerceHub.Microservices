using Ordering.Application.Interfaces;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers;

public class GetOrderListQueryMapper : IQueryMapper<List<Order>, List<OrderResponse>> , IMapper
{
    public List<OrderResponse> MapFromEntity(List<Order> entity)
    {
        List<OrderResponse> OrderListResponse= new List<OrderResponse>();
        entity.ForEach(e =>
        {
            OrderListResponse.Add(new OrderResponse()
            {
                Id = e.Id,
                UserName = e.UserName,
                TotalPrice = e.TotalPrice,
                FirstName = e.FirstName,
                LastName = e.LastName,
                EmailAddress = e.EmailAddress,
                AddressLine = e.AddressLine,
                Country = e.Country,
                State = e.State,
                ZipCode = e.ZipCode,
                CardName = e.CardName,
                CardNumber = e.CardNumber,
                Expiration = e.Expiration,
                Cvv = e.Cvv,
                PaymentMethod = e.PaymentMethod
            });
        });
        return OrderListResponse;
    }
}
