using Products.Application.Common.Models;
using Products.Application.Dtos;

namespace Products.Application.UseCases.Interfaces;

public interface IGetProductByIdUseCase
{
    Task<Result<ProductDto?>> ExecuteAsync(string id, CancellationToken cancellationToken = default);
}
