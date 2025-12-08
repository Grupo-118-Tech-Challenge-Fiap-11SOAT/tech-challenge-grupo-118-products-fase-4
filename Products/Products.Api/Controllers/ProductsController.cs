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
        private readonly IGetProductByTypeUseCase _getProductByTypeUseCase;
        private readonly ICreateProductUseCase _createProductUseCase;

        public ProductsController(ILogger<ProductsController> logger, IGetProductByIdUseCase getProductByIdUseCase, 
            IGetProductByTypeUseCase getProductByTypeUseCase,
            ICreateProductUseCase createProductUseCase)
        {
            _logger = logger;
            _getProductByIdUseCase = getProductByIdUseCase;
            _getProductByTypeUseCase = getProductByTypeUseCase;
            _createProductUseCase = createProductUseCase;
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("{productId}"), ActionName("GetDetailedProduct")]
        [AllowAnonymous]
        public async Task<Result<ProductDto?>> GetAsync(string productId, CancellationToken cancellationToken)
        {
            var result = await _getProductByIdUseCase.ExecuteAsync(productId, cancellationToken);
            return result;
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Result<ProductDto>>> PostAsync(CancellationToken cancellationToken, [FromBody] ProductDto productDto)
        {
            var result = await _createProductUseCase.ExecuteAsync(productDto, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<Result<List<ProductDto>?>> GetByTypeAsync(string category, CancellationToken cancellationToken)
        {
            var result = await _getProductByTypeUseCase.ExecuteAsync(category, cancellationToken);
            return result;
        }
    }
}
