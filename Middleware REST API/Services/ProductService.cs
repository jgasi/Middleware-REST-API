using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;

namespace Middleware_REST_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        { 
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
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
