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
        bool checkExist = collection.Find(_ => true).Any();
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



        if (!checkExist)
        {
            try
            {
                IEnumerable<T>? DataSerilized = GetDeserializeData<T>(fileToRead) ?? null;
                if (DataSerilized!.Any() && DataSerilized != null)
                    foreach (var item in DataSerilized)
                        collection.InsertOneAsync(item);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }





    }

    //  after adding json files as embeded resourses 
    public static IEnumerable<T>? GetDeserializeData<T>(string destinationFile)
    {

        Assembly assemblyName =  Assembly.GetExecutingAssembly();
        var resourceName = $"{assemblyName.GetName().Name}.Data.SeedData.{destinationFile}";

        using Stream? stream = assemblyName.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Resource {resourceName} not found.");

        using StreamReader reader =  new StreamReader(stream);
        string content = reader.ReadToEnd();

        return JsonSerializer.Deserialize<IEnumerable<T>>(content);
    }



}
