using Products.Domain.Entities;

namespace Products.Infra.DataBase.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<Product>?> GetProductByTypeAsync(string type, CancellationToken cancellationToken = default);

    Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);

    Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default);
}
