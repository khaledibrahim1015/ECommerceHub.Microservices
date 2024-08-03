using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepostitory _productRepostitory;

    public CreateProductHandler(IProductRepostitory productRepostitory)
    {
        _productRepostitory = productRepostitory;
    }
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = MapperExtension.Mapper.Map<Product>(request);
        if (productEntity is null)
            throw new ApplicationException("there is an issue with mapping while creating new product");
        Product newProduct =await  _productRepostitory.CreateProduct(productEntity);

        ProductResponse productResponse =  MapperExtension.Mapper.Map<ProductResponse>(newProduct);
        return productResponse;


    }
}
