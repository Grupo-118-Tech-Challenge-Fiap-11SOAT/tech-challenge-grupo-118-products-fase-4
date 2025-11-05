using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.Application.UseCases;

public class GetProductByIdUseCase : IGetProductByIdUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto?>> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        Result<ProductDto?> result = new Result<ProductDto?>();

        var persistedProduct = await _productRepository.GetProductByIdAsync(id, cancellationToken);

        if (persistedProduct == null)       
            return result.Fail("Product not found", HttpStatusCode.NotFound);

        ProductDto product = new ProductDto(persistedProduct.Id, 
            persistedProduct.Name, persistedProduct.Price, persistedProduct.IsActive, persistedProduct.Images);

        return result.Ok(product, HttpStatusCode.OK);
    }
}
