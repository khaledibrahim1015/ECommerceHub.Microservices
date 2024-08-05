using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Prodcuts { get; }

    public IMongoCollection<ProductBrand> Brands { get; }

    public IMongoCollection<ProductType> Types { get; }
    public CatalogContext(IConfiguration configuration)
    {
        var x = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
        IMongoClient client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
        IMongoDatabase db = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

        Prodcuts = db.GetCollection<Product>
                                    (configuration["DatabaseSettings:ProdcutCollection"]);
        Brands = db.GetCollection<ProductBrand>
                                    (configuration["DatabaseSettings:BrandsCollection"]);
        Types = db.GetCollection<ProductType>
                                    (configuration["DatabaseSettings:TypesCollection"]);
        GenericContextSeed.SeedData<Product>(Prodcuts);
        GenericContextSeed.SeedData<ProductBrand>(Brands);
        GenericContextSeed.SeedData<ProductType>(Types);


    }



}
