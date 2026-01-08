using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Examples;
using Products.Api.Extensions;
using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Products.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing products.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IGetProductByIdUseCase _getProductByIdUseCase;
        private readonly IGetProductsUseCase _getProductsUseCase;
        private readonly IGetProductByTypeUseCase _getProductByTypeUseCase;
        private readonly ICreateProductUseCase _createProductUseCase;
        private readonly IGetActiveProductsByIdsUseCase _getActiveProductByIdsUseCase;

        public ProductsController(ILogger<ProductsController> logger, IGetProductByIdUseCase getProductByIdUseCase,
            IGetProductByTypeUseCase getProductByTypeUseCase,
            IGetProductsUseCase getProductsUseCase,
            ICreateProductUseCase createProductUseCase,
            IGetActiveProductsByIdsUseCase getActiveProductByIdsUseCase)
        {
            _logger = logger;
            _getProductByIdUseCase = getProductByIdUseCase;
            _getProductsUseCase = getProductsUseCase;
            _getProductByTypeUseCase = getProductByTypeUseCase;
            _createProductUseCase = createProductUseCase;
            _getActiveProductByIdsUseCase = getActiveProductByIdsUseCase;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="productDto">The product data transfer object containing information about the product to create.</param>
        /// <returns>The created Product.</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(ProductDto), typeof(ProductsExample))]
        [HttpPost]
        public async Task<IActionResult> PostAsync(CancellationToken cancellationToken,
            [FromBody] ProductDto productDto)
        {
            var result = await _createProductUseCase.ExecuteAsync(productDto, cancellationToken);
            return this.ToActionResult(result);
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
        public async Task<IActionResult> GetByIdAsync(string productId, CancellationToken cancellationToken)
        {
            var result = await _getProductByIdUseCase.ExecuteAsync(productId, cancellationToken);
            return this.ToActionResult(result);
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
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var result = await _getProductsUseCase.ExecuteAsync(cancellationToken);
            return this.ToActionResult(result);
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
        public async Task<IActionResult> GetByTypeAsync(string category,
            CancellationToken cancellationToken)
        {
            var result = await _getProductByTypeUseCase.ExecuteAsync(category, cancellationToken);
            return this.ToActionResult(result);
        }

        /// <summary>
        /// Retrieves active products by a list of product IDs.
        /// </summary>
        /// <param name="request">List of product IDs.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of active products.</returns>
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("active/by-ids")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveByIdsAsync([FromBody] List<string> ids, CancellationToken cancellationToken)
        {
            var result = await _getActiveProductByIdsUseCase.ExecuteAsync(ids, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}