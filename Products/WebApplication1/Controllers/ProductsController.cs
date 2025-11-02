using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IGetProductByIdUseCase _getProductByIdUseCase;

        public ProductsController(ILogger<ProductsController> logger, IGetProductByIdUseCase getProductByIdUseCase)
        {
            _logger = logger;
            _getProductByIdUseCase = getProductByIdUseCase;
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("{productId}"), ActionName("GetDetailedProduct")]
        [AllowAnonymous]
        public async Task<Result<ProductDto?>> GetAsync(string productId, CancellationToken cancellationToken)
        {
            var result = await _getProductByIdUseCase.ExecuteAsync(productId, cancellationToken);
            return result;
        }
    }
}
