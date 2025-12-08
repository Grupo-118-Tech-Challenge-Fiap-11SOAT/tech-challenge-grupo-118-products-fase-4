using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
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
        Result<ProductDto> result = new();

        try
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
                return result.Fail("Product not found", HttpStatusCode.NotFound);

            ProductDto dto = product switch
            {
                Snack s => new SnackDto(s.Id, s.Name, s.Price, s.IsActive, s.Images, s.Ingredients),
                Accompaniment a => new AccompanimentDto(a.Id, a.Name, a.Price, a.IsActive, a.Images, a.Size),
                Dessert d => new DessertDto(d.Id, d.Name, d.Price, d.IsActive, d.Images, d.PortionSize),
                Drink dr => new DrinkDto(dr.Id, dr.Name, dr.Price, dr.IsActive, dr.Images, dr.Size, dr.Flavor),
                _ => throw new NotSupportedException("Unsupported product type")
            };

            return result.Ok(dto, HttpStatusCode.OK);
        }
        catch (Exception)
        {
            return result.Fail("Internal Error", HttpStatusCode.InternalServerError);
        }
    }
}
