using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.interfaces;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToDelete != null)
        {
            await _orderRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order with id: {orderToDelete.Id} is successfuly deleted ! ");
            return Unit.Value;
        }
        throw new OrderNotFoundException(typeof(Order).Name, request.Id);
    }
}
