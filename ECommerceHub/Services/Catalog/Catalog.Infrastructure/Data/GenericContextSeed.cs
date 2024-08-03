using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data;

public static class GenericContextSeed
{
    public static void SeedData<T>(IMongoCollection<T> collection) where T : class
    {
        bool checkExist =  collection.Find(_ => true).Any();
        // Determine the file name based on the type of T
        var fileToRead = typeof(T) == typeof(Product)
            ? "products.json"
            : (typeof(T) == typeof(ProductBrand)
                ? "brands.json"
                : (typeof(T) == typeof(ProductType)
                    ? "types.json"
                    : null));



        if (string.IsNullOrEmpty(fileToRead))
        {
            throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}");
        }
        string fullpath = Path.Combine("C:\\Users\\ZALL-TECH\\Desktop\\EShopping\\ECommerceHub.Microservices\\ECommerceHub\\Services\\Catalog\\"
                        , Assembly.GetExecutingAssembly().GetName().Name
                        , "Data", "SeedData", fileToRead
            );

        if (!checkExist)
        {
            try
            {
                string data = File.ReadAllText(fullpath);
                IEnumerable<T>? DataSerilized = JsonSerializer.Deserialize<IEnumerable<T>>(data);
                if (DataSerilized!.Any() && DataSerilized != null)
                    foreach (var item in DataSerilized)
                        collection.InsertOneAsync(item);
            }
            catch (Exception ex )
            {

                Console.WriteLine(ex.Message );
            }
        
        }





    }


 



}
