using Microsoft.Identity.Client;
using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Newtonsoft.Json;

namespace Middleware_REST_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        { 
            _productRepository = productRepository;
        }


        // Methods for API operations

        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            return await _productRepository.GetAllProductsFromExternalApi();
        }

        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {
            return await _productRepository.GetProductByIdFromExternalApi(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice)
        {
            return await _productRepository.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            return await _productRepository.GetProductsByCategoryFromExternalApi(category);
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice)
        {
            return await _productRepository.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            return await _productRepository.SearchProductsByNameFromExternalApi(name);
        }


        // Methods for possible future database operations

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRange(string category, decimal minPrice, decimal maxPrice)
        {
            return await _productRepository.GetProductsByCategoryAndPriceRange(category, minPrice, maxPrice);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            return await _productRepository.GetProductsByCategory(category);
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _productRepository.GetProductsByPriceRange(minPrice, maxPrice);
        }

        public async Task<IEnumerable<Product>> SearchProductsByName(string name)
        {
            return await _productRepository.SearchProductsByName(name);
        }
    }
}
