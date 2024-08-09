using Discount.Application.Interfaces;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IQueryMapper<Coupon, CouponModel> _queryMapper;
    public GetDiscountQueryHandler(IDiscountRepository discountRepository
        , IQueryMapper<Coupon, CouponModel> queryMapper)
    {
        _discountRepository = discountRepository;
        _queryMapper = queryMapper;
    }

    public async  Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with the product name = {request.ProductName} not found"));
        }
        return _queryMapper.MapFromEntity(coupon);

    }
}
