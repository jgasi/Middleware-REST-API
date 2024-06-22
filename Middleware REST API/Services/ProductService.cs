using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Newtonsoft.Json;

namespace Middleware_REST_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProductService> _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public ProductService(IProductRepository productRepository, IMemoryCache cache, ILogger<ProductService> logger) 
        { 
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }


        // Methods for API operations

        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            string cacheKey = $"AllProducts";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetAllProductsFromExternalApi();

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found from external API.");
                }

                _logger.LogInformation("Cache miss for all products. Fetching data from external API.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions); 
            }
            else
            {
                _logger.LogInformation("Cache hit for all products. Fetching data from Cache.");
            }


            return products;
        }

        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {
            string cacheKey = $"Product-{id}";

            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _productRepository.GetProductByIdFromExternalApi(id);
                _logger.LogInformation($"Cache miss for product with ID '{id}'. Fetching data from external API.");


                if (product != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, product, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for product with ID '{id}'. Fetching data from Cache.");
            }


            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredProducts-{category}-{minPrice}-{maxPrice}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Category '{category}' and price range '{minPrice}-{maxPrice}' from external API.");
                }

                _logger.LogInformation($"Cache miss for products with Category '{category}' and price range '{minPrice}-{maxPrice}'. Fetching data from external API.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Category '{category}' and Price range '{minPrice}-{maxPrice}'. Fetching data from Cache.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            string cacheKey = $"FilteredProducts-{category}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategoryFromExternalApi(category);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Category '{category}' from external API.");
                }

                _logger.LogInformation($"Cache miss for products with Category '{category}'. Fetching data from external API.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Category '{category}'. Fetching data from Cache.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredProducts-{minPrice}-{maxPrice}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Price range '{minPrice}-{maxPrice}' from external API.");
                }

                if (products == null)
                {
                    return Enumerable.Empty<Product>();
                }

                _logger.LogInformation($"Cache miss for products with Price range '{minPrice}-{maxPrice}'. Fetching data from external API.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Price range '{minPrice}-{maxPrice}'. Fetching data from Cache.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            string cacheKey = $"FilteredProducts-{name}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.SearchProductsByNameFromExternalApi(name);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products containing '{name}' in Title found from external API.");
                }

                _logger.LogInformation($"Cache miss for products containing '{name}' in Title. Fetching data from external API.");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration);

                _cache.Set(cacheKey, products, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Cache hit for products containing '{name}' in Title. Fetching data from Cache.");
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
            string cacheKey = $"DbProduct-{id}";
            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _productRepository.GetProductById(id);
                _logger.LogInformation($"Cache miss for product with ID '{id}'. Fetching data from Local Database.");

                if(product != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, product, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for product with ID '{id}'. Fetching data from Cache.");
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRange(string category, decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredDbProducts-{category}-{minPrice}-{maxPrice}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategoryAndPriceRange(category, minPrice, maxPrice);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Category '{category}' and price range '{minPrice}-{maxPrice}' from Local Database.");
                }

                _logger.LogInformation($"Cache miss for products with Category '{category}' and Price range '{minPrice}-{maxPrice}'. Fetching data from Local Database.");

                if (products != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, products, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Category '{category}' and Price range '{minPrice}-{maxPrice}'. Fetching data from Cache.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            string cacheKey = $"FilteredDbProducts-{category}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByCategory(category);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Category '{category}' from Local Database.");
                }

                _logger.LogInformation($"Cache miss for products with Category '{category}'. Fetching data from Local Database.");

                if (products != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, products, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Category '{category}'. Fetching data from Cache.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            string cacheKey = $"FilteredDbProducts-{minPrice}-{maxPrice}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetProductsByPriceRange(minPrice, maxPrice);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found with Price range '{minPrice}-{maxPrice}' from Local Database.");
                }

                _logger.LogInformation($"Cache miss for products with Price range '{minPrice}-{maxPrice}'. Fetching data from Local Database.");

                if (products != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, products, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for products with Price range '{minPrice}-{maxPrice}'. Fetching data from Cache.");
            }



            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsByName(string name)
        {
            string cacheKey = $"FilteredDbProducts-{name}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.SearchProductsByName(name);

                if (products == null)
                {
                    throw new ProductNotFoundException($"No products found containing '{name}' in Title from Local Database.");
                }

                _logger.LogInformation($"Cache miss for products containing '{name}' in Title. Fetching data from Local Database.");

                if (products != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(_cacheDuration);

                    _cache.Set(cacheKey, products, cacheEntryOptions);
                }
            }
            else
            {
                _logger.LogInformation($"Cache hit for products containing '{name}' in Title. Fetching data from Cache.");
            }


            return products;
        }
    }
}
