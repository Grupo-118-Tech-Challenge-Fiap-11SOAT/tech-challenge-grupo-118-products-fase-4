using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;


[BsonDiscriminator("Drink")]
public class Drink : Product
{
    public Drink(string name, decimal price, bool isActive, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
    }

    public Drink(ObjectId id ,string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public string Size { get; set; } = null!;
    public string? Flavor { get; set; }
}
