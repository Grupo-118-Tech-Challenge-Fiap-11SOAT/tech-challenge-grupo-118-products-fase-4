using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.Application.UseCases;

public class GetProductByTypeUseCase : IGetProductByTypeUseCase
{
    private readonly IProductRepository _productRepository;
    public GetProductByTypeUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>?>> ExecuteAsync(string type, CancellationToken cancellationToken = default)
    {
        Result<List<ProductDto>?> result = new Result<List<ProductDto>?>();
        var products = new List<ProductDto>();

        var persistedProducts = await _productRepository.GetProductByTypeAsync(type, cancellationToken);

        if (persistedProducts == null)
            return result.Fail("Products not found", HttpStatusCode.NotFound);

        foreach (var p in persistedProducts)
        {
            ProductDto product = new ProductDto(p.Id,
            p.Name, p.Price, p.IsActive, p.Images);

            products.Add(product);
        }

        return result.Ok(products, HttpStatusCode.OK);
    }
}
