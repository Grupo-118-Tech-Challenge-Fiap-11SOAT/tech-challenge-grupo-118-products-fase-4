using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.Application.UseCases;

public class GetProductsUseCase : IGetProductsUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result<List<ProductDto>> result = new();

        try
        {
            var products = await _productRepository.GetProductsAsync(cancellationToken);
            var response = new List<ProductDto>();

            foreach (var item in products)
            {
                ProductDto dto = item switch
                {
                    Snack s => new SnackDto(s.Id, s.Name, s.Price, s.IsActive, s.Images, s.Ingredients),
                    Accompaniment a => new AccompanimentDto(a.Id, a.Name, a.Price, a.IsActive, a.Images, a.Size),
                    Dessert d => new DessertDto(d.Id, d.Name, d.Price, d.IsActive, d.Images, d.PortionSize),
                    Drink dr => new DrinkDto(dr.Id, dr.Name, dr.Price, dr.IsActive, dr.Images, dr.Size, dr.Flavor),
                    _ => throw new NotSupportedException("Unsupported product type")
                };

                response.Add(dto);
            }

            return result.Ok(response, HttpStatusCode.OK);
        }
        catch (Exception)
        {
            return result.Fail("Internal Error", HttpStatusCode.InternalServerError);
        }
    }
}
