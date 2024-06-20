using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Newtonsoft.Json;

namespace Middleware_REST_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public ProductService(IProductRepository productRepository, IMemoryCache cache) 
        { 
            _productRepository = productRepository;
            _cache = cache;
        }


        // Methods for API operations

        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            string cacheKey = $"AllProducts";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetAllProductsFromExternalApi();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }


            return products;
        }

        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {
            string cacheKey = $"Product-{id}";

            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _productRepository.GetProductByIdFromExternalApi(id);

                if (product != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, product, cacheEntryOptions);
                }
            }


            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredProducts-{category}-{minPrice}-{maxPrice}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            string cacheKey = $"FilteredProducts-{category}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategoryFromExternalApi(category);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredProducts-{minPrice}-{maxPrice}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            string cacheKey = $"FilteredProducts-{name}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.SearchProductsByNameFromExternalApi(name);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }


            return products;
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
