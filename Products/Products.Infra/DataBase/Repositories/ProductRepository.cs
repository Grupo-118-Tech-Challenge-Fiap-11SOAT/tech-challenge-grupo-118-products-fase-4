using MongoDB.Bson;
using MongoDB.Driver;
using Products.Domain.Common.Exceptions;
using Products.Domain.Entities;
using Products.Infra.DataBase.Contexts;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Products.Infra.DataBase.Repositories;

[ExcludeFromCodeCoverage]
public class ProductRepository : IProductRepository
{
    private readonly MongoDbContext _context;

    public ProductRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            throw new InvalidObjectIdException(id);

        return await _context.Products.Find(x => x.Id == objectId).FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetActiveProductsByIdsAsync(
    List<string> ids,
    CancellationToken cancellationToken = default)
    {
        var objectIds = new List<ObjectId>();

        foreach (var id in ids)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                throw new InvalidObjectIdException(id);

            objectIds.Add(objectId);
        }

        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.In(x => x.Id, objectIds),
            Builders<Product>.Filter.Eq(x => x.IsActive, true)
        );

        return await _context.Products
            .Find(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }

    public async Task<List<Product>?> GetProductByTypeAsync(string type,CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq("_t", type);

        var options = new FindOptions<Product>
        {
            Collation = new Collation(
                locale: "en",
                strength: CollationStrength.Secondary
            )
        };

        using var cursor = await _context.Products.FindAsync(filter, options, cancellationToken);

        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task ClearProductsAsync(CancellationToken cancellationToken = default)
    {
        await _context.Products.DeleteManyAsync(
            Builders<Product>.Filter.Empty,
            cancellationToken
        );
    }

    public async Task CreateManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken)
    {
        await _context.Products.InsertManyAsync(
        products,
        new InsertManyOptions
        {
            IsOrdered = true
        },
        cancellationToken);
    }
}
