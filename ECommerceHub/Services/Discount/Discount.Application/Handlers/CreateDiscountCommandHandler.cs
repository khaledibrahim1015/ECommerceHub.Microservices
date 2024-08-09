using Discount.Application.Commands;
using Discount.Application.Interfaces;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ICommandMapper<CreateDiscountCommand , Coupon> _commandMapper;
    private readonly IQueryMapper<Coupon, CouponModel> _queryMapper;

    public CreateDiscountCommandHandler(
          IDiscountRepository discountRepository
        , IQueryMapper<Coupon, CouponModel> queryMapper,
            ICommandMapper<CreateDiscountCommand, Coupon> commandMapper
        )
    {
        _discountRepository = discountRepository;
        _queryMapper = queryMapper;
        _commandMapper = commandMapper;
    }

    public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = _commandMapper.MapToEntity(request);
        var res =await _discountRepository.CreateDiscount(coupon);
        return _queryMapper.MapFromEntity(coupon);
        
    }
}
