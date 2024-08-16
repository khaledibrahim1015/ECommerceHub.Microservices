using EventBus.Messages.Events;
using Ordering.Application.Commands;
using Ordering.Application.Interfaces;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers;

public class CheckoutOrderCommandMapper : IMapper, ICommandMapper<CheckoutOrderCommand, Order>
                                            , IEventCommandMapper<BasketCheckoutEvent, CheckoutOrderCommand>
{
    public CheckoutOrderCommand MapToCommand(BasketCheckoutEvent @event)
    {
        return new CheckoutOrderCommand()
        {
            UserName = @event.UserName,
            TotalPrice = @event.TotalPrice,
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            EmailAddress = @event.EmailAddress,
            AddressLine = @event.AddressLine,
            Country = @event.Country,
            State = @event.State,
            ZipCode = @event.ZipCode,
            CardName = @event.CardName,
            CardNumber = @event.CardNumber,
            Expiration = @event.Expiration,
            Cvv = @event.Cvv,
            PaymentMethod = @event.PaymentMethod



        };
    }

    public Order MapToEntity(CheckoutOrderCommand command)
    {
        return new Order()
        {
            UserName = command.UserName,
            TotalPrice = command.TotalPrice,
            FirstName = command.FirstName,
            LastName = command.LastName,
            EmailAddress = command.EmailAddress,
            AddressLine = command.AddressLine,
            Country = command.Country,
            State = command.State,
            ZipCode = command.ZipCode,
            CardName = command.CardName,
            CardNumber = command.CardNumber,
            Expiration = command.Expiration,
            Cvv = command.Cvv,
            PaymentMethod = command.PaymentMethod
        };
    }
}
