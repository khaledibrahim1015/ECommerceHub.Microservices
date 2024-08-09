using Discount.Application.Commands;
using Discount.Application.Interfaces;
using Discount.Core.Entities;

namespace Discount.Application.Mappers;

public class UpdateDiscountCommandMapper : ICommandMapper<UpdateDiscountCommand, Coupon>, IMapper
{
 

    public Coupon MapToEntity(UpdateDiscountCommand command)
    {
        return new Coupon
        {
            Id = command.Id,
            ProductName = command.ProductName,
            Amount = command.Amount,
            Description = command.Description,
        };
    }
}
