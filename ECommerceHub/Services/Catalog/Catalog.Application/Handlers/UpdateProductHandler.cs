using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepostitory _productRepostitory;

    public UpdateProductHandler(IProductRepostitory productRepostitory)
    {
       _productRepostitory = productRepostitory;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        bool  productEntity = await _productRepostitory.UpdateProduct(new Product()
        {
            Id = request.Id,
            Name = request.Name,
            ImageFile = request.ImageFile,
            Price = request.Price,
            Description = request.Description,
            Summary = request.Summary,
            Brands = request.Brands,
            Types = request.Types,
        });
        return productEntity;
    }
}
