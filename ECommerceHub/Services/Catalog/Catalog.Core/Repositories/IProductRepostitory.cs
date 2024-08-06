using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories;

public  interface IProductRepostitory
{
    Task<Pagination<Product>> GetProductPaginationAsync(CatalogSpecsParams catalogSpecsParams);
    Task<Product> GetProduct(string id);

    Task<IEnumerable<Product>> GetProducts();
    Task<IEnumerable<Product>> GetProductByName(string name);
    Task<IEnumerable<Product>> GetProductByBrand(string name);

    Task<Product> CreateProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);


}
