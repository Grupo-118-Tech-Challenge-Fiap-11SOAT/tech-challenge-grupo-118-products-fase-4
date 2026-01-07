using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Domain.Entities;

namespace Products.Application.UseCases.Interfaces;

public interface ICreateProductUseCase
{
    Task<Result<ProductDto>> ExecuteAsync(ProductDto productDto, CancellationToken cancellationToken);
}
