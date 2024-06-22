using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;

namespace Middleware_REST_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductService> _logger;

        public ProductsController(IProductService productService, ILogger<ProductService> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        // Methods for API operations

        [HttpGet("api")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsFromExternalApi()
        {
            try
            {
                var products = await _productService.GetAllProductsFromExternalApi();
                return Ok(products);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("api/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdFromExternalApi(string id)
        {
            try
            {
                if (!int.TryParse(id, out int productId))
                {
                    _logger.LogWarning($"Invalid product ID format: '{id}'.");
                    return BadRequest($"Invalid product ID format: '{id}'.");
                }

                var product = await _productService.GetProductByIdFromExternalApi(productId);
                return Ok(product);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("api/category/{category}/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, [FromQuery] string minPrice, [FromQuery] string maxPrice)
        {
            try
            {
                if (!decimal.TryParse(minPrice, out decimal productMinPrice) || !decimal.TryParse(maxPrice, out decimal productMaxPrice))
                {
                    _logger.LogWarning($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                    return BadRequest($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                }

                var products = await _productService.GetProductsByCategoryAndPriceRangeFromExternalApi(category, productMinPrice, productMaxPrice);
                return Ok(products);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("api/category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryFromExternalApi(string category)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryFromExternalApi(category);
                return Ok(products);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("api/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRangeFromExternalApi([FromQuery] string minPrice, [FromQuery] string maxPrice)
        {
            try
            {
                if (!decimal.TryParse(minPrice, out decimal productMinPrice) || !decimal.TryParse(maxPrice, out decimal productMaxPrice))
                {
                    _logger.LogWarning($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                    return BadRequest($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                }

                var products = await _productService.GetProductsByPriceRangeFromExternalApi(productMinPrice, productMaxPrice);
                return Ok(products);
            }
            catch(ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("api/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByNameFromExternalApi([FromQuery] string name)
        {
            try
            {
                var products = await _productService.SearchProductsByNameFromExternalApi(name);
                return Ok(products);
            }
            catch(ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        // Methods for possible future database operations

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            try
            {
                if (!int.TryParse(id, out int productId))
                {
                    _logger.LogWarning($"Invalid product ID format: '{id}'.");
                    return BadRequest($"Invalid product ID format: '{id}'.");
                }

                var product = await _productService.GetProductById(productId);
                return Ok(product);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("category/{category}/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAndPriceRange(string category, [FromQuery] string minPrice, [FromQuery] string maxPrice)
        {
            try
            {
                if (!decimal.TryParse(minPrice, out decimal productMinPrice) || !decimal.TryParse(maxPrice, out decimal productMaxPrice))
                {
                    _logger.LogWarning($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                    return BadRequest($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                }

                var products = await _productService.GetProductsByCategoryAndPriceRange(category, productMinPrice, productMaxPrice);
                return Ok(products);
            }
            catch(ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            try
            {
                var products = await _productService.GetProductsByCategory(category);
                return Ok(products);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange([FromQuery] string minPrice, [FromQuery] string maxPrice)
        {
            try
            {
                if (!decimal.TryParse(minPrice, out decimal productMinPrice) || !decimal.TryParse(maxPrice, out decimal productMaxPrice))
                {
                    _logger.LogWarning($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                    return BadRequest($"Invalid product Price range format: '{minPrice}-{maxPrice}'.");
                }

                var products = await _productService.GetProductsByPriceRange(productMinPrice, productMaxPrice);
                return Ok(products);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByName([FromQuery] string name)
        {
            try
            {
                var products = await _productService.SearchProductsByName(name);
                return Ok(products);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
