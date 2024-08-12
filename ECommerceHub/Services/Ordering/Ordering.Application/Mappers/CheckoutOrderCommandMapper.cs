using Ordering.Application.Commands;
using Ordering.Application.Interfaces;
using Ordering.Core.Entities;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Windows.Input;

namespace Ordering.Application.Mappers;

public class CheckoutOrderCommandMapper : IMapper, ICommandMapper<CheckoutOrderCommand, Order>
{

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
