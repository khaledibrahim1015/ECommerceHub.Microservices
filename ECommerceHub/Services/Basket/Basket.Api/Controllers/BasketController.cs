using Basket.Application.Commands;
using Basket.Application.DiscountService;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

public class BasketController: ApiController
{
    private readonly IMediator _mediator;
    

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name ="GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var shoppingCartResponse = await  _mediator.Send(query);
        return Ok(shoppingCartResponse);
    }

    [HttpPost]
    [Route("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse) , (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket
        ( [FromBody] CreateShoppingCartCommand createShoppingCartCommand )
    {

        var basket=  await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);

    }

    [HttpDelete]
    [Route("[action]/{userName}", Name ="DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        var command =  new DeleteBasketByUserNameCommand(userName);
        await _mediator.Send(command);
        return Ok();
    }





}
