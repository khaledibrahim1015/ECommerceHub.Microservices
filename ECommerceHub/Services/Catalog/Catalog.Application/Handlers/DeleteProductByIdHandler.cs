using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdQuery, bool>
{
    private readonly IProductRepostitory _productRepostitory;

    public DeleteProductByIdHandler(IProductRepostitory productRepostitory)
    {
       _productRepostitory = productRepostitory;
    }
    public async Task<bool> Handle(DeleteProductByIdQuery request, CancellationToken cancellationToken)
    {
        var productEntity = await _productRepostitory.DeleteProduct(request.Id);
        return productEntity;

    }
}
