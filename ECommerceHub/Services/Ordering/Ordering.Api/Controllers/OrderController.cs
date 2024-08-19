using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.Api.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public OrderController(IMediator mediator,
                ILogger<OrderController> logger,
                ICorrelationIdGenerator correlationIdGenerator)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {correlationId}", correlationIdGenerator.Get());
        }

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        //  for testing only 
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand checkoutOrderCommand)
        {

            var result = await _mediator.Send(checkoutOrderCommand); return Ok(result);
        }


        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            var result = await _mediator.Send(updateOrderCommand);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            var result = await _mediator.Send(command);
            return NoContent();
        }

    }
}
