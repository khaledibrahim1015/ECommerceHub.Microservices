using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProdcutsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepostitory _productRepostitory;

        public GetAllProductsHandler(IProductRepostitory productRepostitory)
        {
            _productRepostitory = productRepostitory;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProdcutsQuery request, CancellationToken cancellationToken)
        {
            Pagination<Product> paginatedProductList =await _productRepostitory.GetProductPaginationAsync(request._catalogSpecsParams);

            Pagination<ProductResponse> productResponseList = MapperExtension.Mapper.Map<Pagination<ProductResponse>>(paginatedProductList);
            return productResponseList;
        }
    }
}
