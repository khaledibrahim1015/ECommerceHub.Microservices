using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data;

public static class GenericContextSeed
{
    public static void SeedData<T>(IMongoCollection<T> collection) where T : class
    {
        bool checkExist =  collection.Find(_ => true) != null;
        // Determine the file name based on the type of T
        var fileToRead = typeof(T) == typeof(Product)
            ? "products.json"
            : (typeof(T) == typeof(ProductBrand)
                ? "brands.json"
                : (typeof(T) == typeof(ProductType)
                    ? "types.json"
                    : null));

        // Optionally, you could add a check for an empty file name and handle the case where T does not match any known type
        if (string.IsNullOrEmpty(fileToRead))
        {
            throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}");
        }

        string path = Path.Combine("Data", "SeedData", fileToRead);

        if(!checkExist)
        {
            string data = File.ReadAllText(path);
            IEnumerable<T>? DataSerilized =  JsonSerializer.Deserialize<IEnumerable<T>>(data);
            if (DataSerilized!.Any() && DataSerilized != null)
                foreach (var item in DataSerilized)
                    collection.InsertOneAsync(item);
        }





    }



}
