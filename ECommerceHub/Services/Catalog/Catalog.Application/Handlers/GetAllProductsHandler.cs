using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProdcutsQuery, IList<ProductResponse>>
    {
        private readonly IProductRepostitory _productRepostitory;

        public GetAllProductsHandler(IProductRepostitory productRepostitory)
        {
            _productRepostitory = productRepostitory;
        }

        public async Task<IList<ProductResponse>> Handle(GetAllProdcutsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> productList =await _productRepostitory.GetProducts();

            List<ProductResponse> productResponseList = MapperExtension.Mapper.Map<List<ProductResponse>>(productList);
            return productResponseList;
        }
    }
}
