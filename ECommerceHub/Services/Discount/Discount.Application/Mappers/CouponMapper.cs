using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers;

public static  class CouponMapper
{
    public static CouponModel ToGrpcModel(this Coupon coupon)
    {
        return new CouponModel
        {
            Id = coupon.Id,
            ProductName  = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount,
        };
    }
    public static  Coupon FromGrpcModel(this CouponModel couponModel)
    {
        return new Coupon
        {
            Id = couponModel.Id,
            ProductName = couponModel.ProductName,
            Description = couponModel.Description,
            Amount = couponModel.Amount,
        };
    }


}
