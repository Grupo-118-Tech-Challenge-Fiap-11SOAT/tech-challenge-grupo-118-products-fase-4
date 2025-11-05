using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Xml.Linq;

namespace Products.Domain.Entities;

[BsonDiscriminator("Accompaniment")]
public class Accompaniment : Product
{
    public Accompaniment(ObjectId id,string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public Accompaniment(string name, decimal price, bool isActive, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
    }

    public string Size { get; set; } = null!;
}
