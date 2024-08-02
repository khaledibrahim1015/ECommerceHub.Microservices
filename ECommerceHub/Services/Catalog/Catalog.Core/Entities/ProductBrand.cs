using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

internal class ProductBrand :BaseEntity
{
    [BsonElement("Name")]
    public string Name { get; set; }
}
