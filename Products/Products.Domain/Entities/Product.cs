using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.RegularExpressions;

namespace Products.Domain.Entities;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Snack), typeof(Drink), typeof(Dessert), typeof(Accompaniment))]
public abstract class Product
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    [BsonElement("images")]
    public List<ImageProduct> Images { get; protected set; }

    [BsonElement("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    public Product(
        ObjectId id,
        string name,
        decimal price,
        bool isActive,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        List<ImageProduct>? images = null
        )
    {
        Id = id;
        Name = name;
        Price = price;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;

        Images = images ?? new List<ImageProduct>();
    }

    public Product(
        string name,
        decimal price,
        bool isActive,
        List<ImageProduct>? images = null
        )
    {
        Name = name;
        Price = price;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        Images = images ?? new List<ImageProduct>();
    }
}

public class ImageProduct
{
    [BsonElement("position")]
    public int Position { get; protected set; }

    [BsonElement("url")]
    public string Url { get; protected set; }

    private readonly Regex _imageRegex = new Regex(@"(\W)(jpg|jpeg|png|gif|webp)", RegexOptions.Compiled);

    public ImageProduct(int positon, string url)
    {
        Position = positon;
        Url = url;

        CheckImageUrlFormat();
        CheckIfIsAValidPosition();
    }

    private void CheckImageUrlFormat()
    {
        if (!Uri.IsWellFormedUriString(this.Url, UriKind.Absolute))
            throw new Exception("Invalid Url.");

        if (!_imageRegex.IsMatch(this.Url))
            throw new("The provided URL is not valid format.");
    }

    private void CheckIfIsAValidPosition()
    {
        if (this.Position <= 0)
            throw new Exception("Image position is invalid");
    }

    
}




