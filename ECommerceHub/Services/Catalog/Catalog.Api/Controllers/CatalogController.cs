using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    public class CatalogController :ApiController
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> GetProdcutById([FromRoute] string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await  _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}",Name = "GetProdcutByProductName")]
        [ProducesResponseType(typeof(List<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProdcutByProductName([FromRoute]string productName)
        {
            var query  =  new GetProductByBrandQuery(productName);
            var result =await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{brand}", Name = "GetProductsByBrandName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrandName([FromRoute] string brand)
        {
            var query = new GetProductByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(Pagination<ProductResponse>),(int) HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecsParams catalogSpecsParams)
        {
            var query  = new GetAllProdcutsQuery(catalogSpecsParams);
            var result = await  _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponse>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
                        =>  Ok(await _mediator.Send(new GetAllBrandsQuery()));

        [HttpGet("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<TypesResponse>>> GetAllTypes()
                   => Ok(await _mediator.Send(new GetAllTypesQuery()));

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand createProduct)
              => Ok(await _mediator.Send(createProduct));

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductCommand updateProduct)
            => Ok(await _mediator.Send(updateProduct));

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id )
            => Ok(await _mediator.Send(new DeleteProductByIdQuery(id)));



    }
}
