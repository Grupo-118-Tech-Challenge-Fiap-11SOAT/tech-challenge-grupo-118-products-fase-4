using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

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

            var imagesDto = new List<ImageProductDto>();

            foreach (var item in product.Images) 
            {
                imagesDto.Add(new ImageProductDto() { Url = item.Url, Position = item.Position});
            }

            ProductDto dto = product switch
            {
                Snack s => new SnackDto() { Id = s.Id, Name = s.Name, Price =s.Price, IsActive = s.IsActive, Images = imagesDto, Ingredients = s.Ingredients },
                Accompaniment a => new AccompanimentDto() { Id = a.Id, Name = a.Name, Price = a.Price, IsActive = a.IsActive, Images = imagesDto, Size = a.Size },
                Dessert d => new DessertDto() { Id = d.Id, Name = d.Name, Price = d.Price, IsActive = d.IsActive, Images = imagesDto, PortionSize = d.PortionSize },
                Drink dr => new DrinkDto() { Id = dr.Id, Name = dr.Name, Price = dr.Price, IsActive = dr.IsActive, Images = imagesDto, Size = dr.Size, Flavor = dr.Flavor },
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
