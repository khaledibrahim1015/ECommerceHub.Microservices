using MediatR;

namespace Ordering.Application.Commands;

public  class CheckoutOrderCommand : IRequest<int>
{
    public CheckoutOrderCommand()
    {  
    }
    public CheckoutOrderCommand(string? userName, decimal? totalPrice,
        string? firstName, string? lastName, string? emailAddress,
        string? addressLine, string? country, string? state, string? zipCode, 
        string? cardName, string? cardNumber, string? expiration, string? cvv,
        int? paymentMethod)
    {
        UserName = userName;
        TotalPrice = totalPrice;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        Cvv = cvv;
        PaymentMethod = paymentMethod;
    }

    public string? UserName { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? AddressLine { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? CardName { get; set; }
    public string? CardNumber { get; set; }
    public string? Expiration { get; set; }
    public string? Cvv { get; set; }
    public int? PaymentMethod { get; set; }
}
