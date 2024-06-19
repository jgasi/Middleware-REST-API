using Microsoft.AspNetCore.Mvc;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;

namespace Middleware_REST_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        // Methods for API operations

        [HttpGet("api")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsFromExternalApi()
        {
            var products = await _productService.GetAllProductsFromExternalApi();
            return Ok(products);
        }

        [HttpGet("api/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdFromExternalApi(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdFromExternalApi(id);
                return Ok(product);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("api/category/{category}/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, [FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await _productService.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("api/category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryFromExternalApi(string category)
        {
            var products = await _productService.GetProductsByCategoryFromExternalApi(category);
            return Ok(products);
        }

        [HttpGet("api/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRangeFromExternalApi([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await _productService.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("api/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByNameFromExternalApi([FromQuery] string name)
        {
            var products = await _productService.SearchProductsByNameFromExternalApi(name);
            return Ok(products);
        }



        // Methods for possible future database operations

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(product);
            }
            catch (ProductNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("category/{category}/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAndPriceRange(string category, [FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await _productService.GetProductsByCategoryAndPriceRange(category, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategory(category);
            return Ok(products);
        }

        [HttpGet("price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await _productService.GetProductsByPriceRange(minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByName([FromQuery] string name)
        {
            var products = await _productService.SearchProductsByName(name);
            return Ok(products);
        }
    }
}
