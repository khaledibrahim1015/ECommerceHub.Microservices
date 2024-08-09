using Discount.Application.Commands;
using Discount.Application.Interfaces;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers;

public  class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _dcountRepository;
    private readonly ICommandMapper<UpdateDiscountCommand , Coupon> _commandMapper;
    private readonly IQueryMapper<Coupon, CouponModel> _queryMapper;
    public UpdateDiscountCommandHandler(
        IDiscountRepository dcountRepository,
        ICommandMapper<UpdateDiscountCommand, Coupon> commandMapper,
        IQueryMapper<Coupon, CouponModel> queryMapper)
    {
        _dcountRepository = dcountRepository;
        _commandMapper = commandMapper;
        _queryMapper = queryMapper;
    }

    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
       var coupon =  _commandMapper.MapToEntity(request);
       await  _dcountRepository.UpdateDiscount(coupon);
        return _queryMapper.MapFromEntity(coupon);
    }
}
