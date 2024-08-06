using Catalog.Core.Consts;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepostitory, IBrandRepository, ITypeRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<Pagination<Product>> GetProductPaginationAsync(CatalogSpecsParams catalogSpecsParams)
        {
            var products =await _context.Prodcuts
                                        .Find(GetFilterDefination(catalogSpecsParams))
                                        .Sort(GetSortedDefination(catalogSpecsParams.Sort))
                                        .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                                        .Limit(catalogSpecsParams.PageSize)
                                        .ToListAsync();


            return new Pagination<Product>()
            {
                PageIndex = catalogSpecsParams.PageIndex,
                PageSize = catalogSpecsParams.PageSize,
                Count = await _context.Prodcuts.CountDocumentsAsync(_ => true),
                Data = products
            };
        }

        private FilterDefinition<Product> GetFilterDefination (CatalogSpecsParams catalogSpecsParams)
        {
            FilterDefinitionBuilder<Product> builder = Builders<Product>.Filter;
            FilterDefinition<Product> filter = builder.Empty;

            // like operation 
            if(!string.IsNullOrEmpty(catalogSpecsParams.Search))
                filter &= builder.Regex(prd => prd.Name , new BsonRegularExpression(catalogSpecsParams.Search));

            if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId))
                filter &= builder.Eq(prd => prd.Brands.Id, catalogSpecsParams.BrandId);

            if(!string.IsNullOrEmpty(catalogSpecsParams.TypeId))
                filter &= builder.Eq(prd => prd.Types.Id , catalogSpecsParams.TypeId);

            return filter;

        }
        private SortDefinition<Product> GetSortedDefination(string sort)
        {
            return sort.ToLower() switch
            {
                OrderBy.OrderByPriceAsc => Builders<Product>.Sort.Ascending(prd => prd.Price),
                OrderBy.OrderByPriceDesc => Builders<Product>.Sort.Descending(prd=> prd.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            } ;

        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            var res  = await _context
                    .Prodcuts
                      .Find(_ => true)
                        .ToListAsync();
            return res;
        } 
            
   

        public async Task<Product> GetProduct(string id)
        {
            FilterDefinition<Product> filter =  Builders<Product>
                                                                .Filter.Eq(p => p.Id , id);
            return  await _context.Prodcuts
                                    .Find(filter)
                                    .FirstOrDefaultAsync();
        }
      
        public async Task<IEnumerable<Product>> GetProductByName(string name) => 
                await _context.Prodcuts
                                    .Find(p => p.Name == name).ToListAsync();

        
        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            FilterDefinition<Product> filter  = Builders<Product>
                                                        .Filter.Eq( p => p.Brands.Name , name);
            return await _context.Prodcuts
                                        .Find(filter).ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
           await _context.Prodcuts
                                .InsertOneAsync(product);
            return product;

        }


        public async Task<bool> UpdateProduct(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                        .Filter.Eq( p=> p.Id , product.Id);
            var updatedResult = await _context.Prodcuts
                                              .ReplaceOneAsync(filter, product, new ReplaceOptions { IsUpsert = true });
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteProduct(string id)
        {
             var deletedResult =  await  _context.Prodcuts
                                                    .DeleteOneAsync(p => p.Id == id);
              return  deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands() => 
                      await _context.Brands
                                    .Find(_ => true).ToListAsync();
 

        public async Task<IEnumerable<ProductType>> GetAllTypes() =>
                 await _context.Types
                                    .Find(_ => true).ToListAsync();







    }
}
