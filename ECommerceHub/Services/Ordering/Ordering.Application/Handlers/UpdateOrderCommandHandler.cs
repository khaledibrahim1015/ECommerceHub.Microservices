using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Application.Interfaces;
using Ordering.Core.Entities;
using Ordering.Core.interfaces;

namespace Ordering.Application.Handlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly ICommandMapper<UpdateOrderCommand, Order> _commandMapper;


    public UpdateOrderCommandHandler(IOrderRepository orderRepository
         , ILogger<UpdateOrderCommandHandler> logger,
          ICommandMapper<UpdateOrderCommand, Order> commandMapper)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _commandMapper = commandMapper;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToUpdate != null)
        {
            var orderMap = _commandMapper.MapToEntity(request);
            await _orderRepository.UpdateAsync(orderMap);
            _logger.LogInformation($"Order {orderMap.Id} is successfuly updated ");

            return Unit.Value;
        }
        throw new OrderNotFoundException(typeof(Order).Name, request.Id);

    }


}
