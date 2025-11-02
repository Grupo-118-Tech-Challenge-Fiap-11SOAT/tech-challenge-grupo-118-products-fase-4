
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;


[BsonDiscriminator("Drink")]
public class Drink : Product
{
    public Drink(string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public string Size { get; set; } = null!;
    public string? Flavor { get; set; }
}
