using Products.Domain.Entities;

namespace Products.Infra.DataBase.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<Product>?> GetProductByTypeAsync(string type, CancellationToken cancellationToken = default);

    Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);

    Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task ClearProductsAsync(CancellationToken cancellationToken = default);
    Task<List<Product>> GetActiveProductsByIdsAsync(List<string> ids, CancellationToken cancellationToken = default);
    Task CreateManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken);

}
