using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

internal class ProductType :BaseEntity
{
    [BsonElement("Name")]
    public string Name { get; set; }
}
