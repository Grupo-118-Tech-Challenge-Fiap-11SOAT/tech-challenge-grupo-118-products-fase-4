using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;

[BsonDiscriminator("Drink")]
public class Drink : Product
{
    [BsonElement("size")]
    public string Size { get; set; } = null!;

    [BsonElement("flavor")]
    public string? Flavor { get; set; }

    public Drink(string name, decimal price, bool isActive, string size, string? flavor = null, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
        Flavor = flavor;
        Size = size;
    }

    public Drink(ObjectId id ,string name, decimal price, bool isActive, string size, DateTimeOffset createdAt, DateTimeOffset updatedAt, string? flavor = null, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
        Flavor = flavor;
        Size = size;
    }
}
