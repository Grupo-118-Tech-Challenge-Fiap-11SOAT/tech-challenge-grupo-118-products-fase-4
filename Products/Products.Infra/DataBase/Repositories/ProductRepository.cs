using MongoDB.Bson;
using MongoDB.Driver;
using Products.Domain.Entities;
using Products.Infra.DataBase.Contexts;
using Products.Infra.DataBase.Repositories.Interfaces;

namespace Products.Infra.DataBase.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MongoDbContext _context;

    public ProductRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var objectId = new ObjectId(id);
        return await _context.Products.Find(x => x.Id == objectId).FirstOrDefaultAsync(); ;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }
}
