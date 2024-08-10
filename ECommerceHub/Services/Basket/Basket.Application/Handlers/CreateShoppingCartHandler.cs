using Basket.Application.Commands;
using Basket.Application.DiscountService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.interfaces;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;
    public CreateShoppingCartHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository;
        _discountGrpcService = discountGrpcService;
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // Consuming DiscountGrpcService To Get CouponByProductName
        request.Items.ForEach(async item =>
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            if (coupon != null)
            {
                item.Price -= coupon.Amount;
            }
        });



        ShoppingCart shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart()
        {
            UserName = request.UserName,
            Items = request.Items,
        });

        ShoppingCartResponse shoppingCartResponse = MapperExtensions.Mapper.Map<ShoppingCartResponse>(shoppingCart);
        return shoppingCartResponse;

    }
}
