using MediatR;
using Ordering.Application.Interfaces;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.interfaces;

namespace Ordering.Application.Handlers;

public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IQueryMapper<List<Order>, List<OrderResponse>> _queryMapper;

    public GetOrderListQueryHandler(IOrderRepository orderRepository,
           IQueryMapper<List<Order>, List<OrderResponse>> queryMapper)
    {
        _orderRepository = orderRepository;
        _queryMapper = queryMapper;
    }

    public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Order> orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
        return _queryMapper.MapFromEntity(orderList.ToList());

    }
}
