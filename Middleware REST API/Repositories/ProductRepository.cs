using System.Net;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Middleware_REST_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ContextDb _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        public ProductRepository(ContextDb context, HttpClient httpClient, ILogger<ProductService> logger) 
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }


        // Methods for API operations
        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/products");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Products not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Products not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for products'.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for products.");
                }
            }


            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            if (products == null || !products.Any())
            {
                _logger.LogWarning($"Products not found.");
                return null;
            }

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return products;
        }


        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {

            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Product with ID '{id}' not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Product with ID '{id}' not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for product ID '{id}'.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for product ID '{id}'.");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(json);

            if (product.Description != null && product.Description.Length > 100)
            {
                product.Description = product.Description.Substring(0, 100);
            }


            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/category/{category}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Products with Category '{category}' and Price range '{minPrice}-{maxPrice}' not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Products with Category '{category}' and Price range '{minPrice}-{maxPrice}' not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for products with Category '{category}' and Price range '{minPrice}-{maxPrice}'.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for products with Category '{category}' and Price range '{minPrice}-{maxPrice}'.");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice);

            if (filteredProducts == null || !filteredProducts.Any())
            {
                _logger.LogWarning($"Products with Category '{category}' and Price range '{minPrice}-{maxPrice}' not found.");
                return null;
            }

            foreach (var product in filteredProducts)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return filteredProducts;
        }


        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/category/{category}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Products with Category '{category}' not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Products with Category '{category}' not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for products with Category '{category}'.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for products with Category '{category}'.");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            if (products == null || !products.Any())
            {
                _logger.LogWarning($"Products with Category '{category}' not found.");
                return null;
            }

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }
            var filteredProducts = products.Where(p => p.Category == category);


            return filteredProducts;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Products with Price range '{minPrice}-{maxPrice}' not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Products with Price range '{minPrice}-{maxPrice}' not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for products with Price range '{minPrice}-{maxPrice}'.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for products with Price range '{minPrice}-{maxPrice}'.");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            if (filteredProducts == null || !filteredProducts.Any())
            {
                _logger.LogWarning($"Products with Price range '{minPrice}-{maxPrice}' not found.");
                return null;
            }

            foreach (var product in filteredProducts)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return filteredProducts;
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/search?q={name}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Products containing {name} in Title not found. HTTP status code: '{response.StatusCode}'.");
                    throw new ProductNotFoundException($"Products containing {name} in Title not found. HTTP status code: '{response.StatusCode}'.");
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code '{response.StatusCode}' for products containing {name} in Title.");
                    throw new Exception($"HTTP request failed with status code '{response.StatusCode}' for products containing {name} in Title'.");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            if (products == null || !products.Any())
            {
                _logger.LogWarning($"Products containing '{name}' in Title not found.");
                return null;
            }

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }
            var filteredProducts = products.Where(p => p.Title.Contains(name, StringComparison.OrdinalIgnoreCase));


            return filteredProducts;
        }



        // Methods for possible future database operations
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (!products.Any())
            {

                _logger.LogWarning($"Products not found.");
                throw new ProductNotFoundException($"Products not found.");
            }

            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID '{id}' not found.");
                throw new ProductNotFoundException($"Product with ID {id} not found");
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRange(string category, decimal minPrice, decimal maxPrice)
        {
            var products = await _context.Products.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
            if (!products.Any()) 
            {
                _logger.LogWarning($"Products with Category '{category}' and Price range '{minPrice}-{maxPrice}' not found.");
                throw new ProductNotFoundException($"Products with Category '{category}' and Price range '{minPrice}-{maxPrice}' not found.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            var products = await _context.Products.Where(p => p.Category == category).ToListAsync();
            if (!products.Any())
            {
                _logger.LogWarning($"Products with Category '{category}' not found.");
                throw new ProductNotFoundException($"Products with Category '{category}' not found.");
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
            if (!products.Any())
            {
                _logger.LogWarning($"Products with Price range '{minPrice}-{maxPrice}' not found.");
                throw new ProductNotFoundException($"Products with Price range '{minPrice}-{maxPrice}' not found.");
            }
            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsByName(string name)
        {
            var products = await _context.Products.Where(p => p.Title.Contains(name)).ToListAsync();
            if (!products.Any())
            {
                _logger.LogWarning($"Products containing '{name}' in Title not found.");
                throw new ProductNotFoundException($"Products containing '{name}' in Title not found.");
            }

            return products;
        }
    }
}
