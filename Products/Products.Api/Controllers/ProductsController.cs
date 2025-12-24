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
        private readonly IGetProductsUseCase _getProductsUseCase;
        private readonly IGetProductByTypeUseCase _getProductByTypeUseCase;
        private readonly ICreateProductUseCase _createProductUseCase;

        public ProductsController(ILogger<ProductsController> logger, IGetProductByIdUseCase getProductByIdUseCase, 
            IGetProductByTypeUseCase getProductByTypeUseCase,
            IGetProductsUseCase getProductsUseCase,
            ICreateProductUseCase createProductUseCase)
        {
            _logger = logger;
            _getProductByIdUseCase = getProductByIdUseCase;
            _getProductsUseCase = getProductsUseCase;
            _getProductByTypeUseCase = getProductByTypeUseCase;
            _createProductUseCase = createProductUseCase;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="productDto">The product data transfer object containing information about the product to create.</param>
        /// <returns>The created Product.</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<Result<ProductDto>> PostAsync(CancellationToken cancellationToken, [FromBody] ProductDto productDto)
        {
            var result = await _createProductUseCase.ExecuteAsync(productDto, cancellationToken);
            return result;
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The product details if found, or a no-content response if not found.</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("{productId}"), ActionName("GetDetailedProduct")]
        [AllowAnonymous]
        public async Task<Result<ProductDto?>> GetByIdAsync(string productId, CancellationToken cancellationToken)
        {
            var result = await _getProductByIdUseCase.ExecuteAsync(productId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of products if available, or a no-content response if no products are found.</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<List<ProductDto>>> GetAsync(CancellationToken cancellationToken)
        {
            var result = await _getProductsUseCase.ExecuteAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Retrieves a list of products by their category (type).
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="category">The category/type of the products to retrieve.</param>
        /// <returns>A list of products that match the specified category.</returns>
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<Result<List<ProductDto>?>> GetByTypeAsync(string category, CancellationToken cancellationToken)
        {
            var result = await _getProductByTypeUseCase.ExecuteAsync(category, cancellationToken);
            return result;
        }
    }
}
