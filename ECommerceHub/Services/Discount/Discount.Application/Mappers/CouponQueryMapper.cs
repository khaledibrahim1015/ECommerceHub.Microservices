using Discount.Application.Interfaces;
using Discount.Core.Entities;
using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Mappers
{
    public class CouponQueryMapper : IQueryMapper<Coupon, CouponModel> , IMapper
    {
        public CouponModel MapFromEntity(Coupon entity)
        {
            return new CouponModel
            {
                Id = entity.Id,
                ProductName = entity.ProductName,
                Description = entity.Description,
                Amount = entity.Amount
            };
        }
    }
}
