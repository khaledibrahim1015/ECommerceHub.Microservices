using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Interfaces;
using Ordering.Core.Entities;
using Ordering.Core.interfaces;

namespace Ordering.Application.Handlers;

internal class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICommandMapper<CheckoutOrderCommand, Order> _comandMapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
        ICommandMapper<CheckoutOrderCommand, Order> comandMapper
        , ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _comandMapper = comandMapper;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderEntity = _comandMapper.MapToEntity(request);
        Order generatedOrder = await _orderRepository.AddAsync(orderEntity);
        _logger.LogInformation($"Order with Generated Id :{generatedOrder} Succefuly Creatred !");

        return generatedOrder.Id;

    }
}
