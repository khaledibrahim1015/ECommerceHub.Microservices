using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByBrandHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
{
    private readonly IProductRepostitory _productRepostitory;

    public GetProductByBrandHandler(IProductRepostitory productRepostitory)
    {
        _productRepostitory = productRepostitory;
    }
    public async Task<IList<ProductResponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> productList = await _productRepostitory.GetProductByName(request.BrandName);
        List<ProductResponse> productResponseList =  MapperExtension.Mapper.Map<List<ProductResponse>>(productList);
        return productResponseList;
    }
}
