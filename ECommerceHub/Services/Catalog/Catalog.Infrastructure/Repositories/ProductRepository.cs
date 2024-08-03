using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepostitory, IBrandRepository, ITypeRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts() => 
            await _context
                    .Prodcuts
                      .Find(_ => true)
                        .ToListAsync();
   

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
