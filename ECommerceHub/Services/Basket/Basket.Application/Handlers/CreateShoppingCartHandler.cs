using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.interfaces;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public CreateShoppingCartHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // Todo: apply DiscountService 

        ShoppingCart shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart()
        {
            UserName = request.UserName,
            Items = request.Items,
        });

        ShoppingCartResponse shoppingCartResponse = MapperExtensions.Mapper.Map<ShoppingCartResponse>(shoppingCart);
        return shoppingCartResponse;

    }
}
