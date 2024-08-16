using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var shoppingCartResponse = await _mediator.Send(query);
        return Ok(shoppingCartResponse);
    }

    [HttpPost]
    [Route("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket
        ([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {

        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);

    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        var command = new DeleteBasketByUserNameCommand(userName);
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [Route("[action]", Name = "checkout")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        //  get basket for existing user
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        ShoppingCartResponse basket = await _mediator.Send(query);
        if (basket is null)
            return BadRequest();

        // if not publish basketcheckoutEvent into Rabbitmq 
        BasketCheckoutEvent basketCheckoutEvent = MapperExtensions.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
        basketCheckoutEvent.TotalPrice = basket.TotalPrice;
        //await _publishEndpoint.Publish(basketCheckoutEvent, context =>
        //{
        //    context.SetRoutingKey(EventBusConstant.BasketCheckoutQueue);
        //});
        await _publishEndpoint.Publish(basketCheckoutEvent);

        // Remove basket 
        var command = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
        await _mediator.Send(command);
        return Accepted();




    }




}
