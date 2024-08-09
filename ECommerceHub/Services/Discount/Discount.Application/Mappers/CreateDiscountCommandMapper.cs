using Discount.Application.Commands;
using Discount.Application.Interfaces;
using Discount.Core.Entities;

namespace Discount.Application.Mappers;

public class CreateDiscountCommandMapper : ICommandMapper<CreateDiscountCommand, Coupon> , IMapper
{
    public Coupon MapToEntity(CreateDiscountCommand command)
    {
        return new Coupon
        {
            ProductName = command.ProductName,
            Description = command.Description,
            Amount = command.Amount,
        };
    }
}
