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
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Prodcuts = db.GetCollection<Product>
                                    (configuration.GetValue<string>("DatabaseSettings:ProdcutCollection"));
        Brands = db.GetCollection<ProductBrand>
                                    (configuration.GetValue<string>("DatabaseSettings:BrandsCollection"));
        Types = db.GetCollection<ProductType>
                                    (configuration.GetValue<string>("DatabaseSettings:TypesCollection"));

        //  Seeding Data 
        GenericContextSeed.SeedData(Prodcuts);
        GenericContextSeed.SeedData(Brands);
        GenericContextSeed.SeedData(Types);
    }



}
