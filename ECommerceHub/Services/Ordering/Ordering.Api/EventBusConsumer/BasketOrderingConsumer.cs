using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Interfaces;
using System.Text.Json;

namespace Ordering.Api.EventBusConsumer;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly IEventCommandMapper<BasketCheckoutEvent, CheckoutOrderCommand> _commandMapper;
    private readonly ILogger<BasketOrderingConsumer> _logger;
    public BasketOrderingConsumer(IMediator mediator,
            IEventCommandMapper<BasketCheckoutEvent, CheckoutOrderCommand> commandMapper,
            ILogger<BasketOrderingConsumer> logger)
    {
        _mediator = mediator;
        _commandMapper = commandMapper;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        try
        {
            using var scope = _logger.BeginScope("consuming Basket CheckOut event for {correlationId}",
                                         context.Message.CorrelationId);

            _logger.LogInformation("Received BasketCheckoutEvent: {@Event}", context.Message);
            var command = _commandMapper.MapToCommand(context.Message);
            int checkoutOrderId = await _mediator.Send(command);
            _logger.LogInformation($"Basket Checkout event Completed !!!{JsonSerializer.Serialize(command)}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming BasketCheckoutEvent");
        }



    }
}
