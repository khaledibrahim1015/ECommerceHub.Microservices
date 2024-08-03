using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
{
    private readonly IProductRepostitory _productRepostitory;

    public GetProductByNameQueryHandler(IProductRepostitory productRepostitory)
    {
        _productRepostitory = productRepostitory;
    }
    public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> productList = await _productRepostitory.GetProductByName(request.Name);
        IList<ProductResponse> productResponseList = MapperExtension.Mapper.Map<IList<ProductResponse>>(productList);
        return productResponseList;
    }
}
