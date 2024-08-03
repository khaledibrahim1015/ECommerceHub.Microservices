using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepostitory _productRepostitory;

    public GetProductByIdQueryHandler(IProductRepostitory productRepostitory)
    {
        _productRepostitory = productRepostitory;
    }


    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product product = await _productRepostitory.GetProduct(request.Id);

        ProductResponse productResponse = MapperExtension.Mapper.Map<ProductResponse>(product);

        return productResponse;
    }
}
