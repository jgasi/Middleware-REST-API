using Microsoft.AspNetCore.Mvc;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;

namespace Middleware_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
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
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
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




        // Methods for API operations

        [HttpGet("external")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsFromExternalApi()
        {
            var products = await _productService.GetAllProductsFromExternalApi();
            return Ok(products);
        }

        [HttpGet("external/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdFromExternalApi(int id)
        {
            var product = await _productService.GetProductByIdFromExternalApi(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpGet("external/category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryFromExternalApi(string category)
        {
            var products = await _productService.GetProductsByCategoryFromExternalApi(category);
            return Ok(products);
        }

        [HttpGet("external/price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRangeFromExternalApi([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await _productService.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("external/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByNameFromExternalApi([FromQuery] string name)
        {
            var products = await _productService.SearchProductsByNameFromExternalApi(name);
            return Ok(products);
        }
    }
}
