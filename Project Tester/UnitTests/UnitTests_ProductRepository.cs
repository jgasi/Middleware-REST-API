using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using Middleware_REST_API.Services;

namespace Project_Tester.UnitTests
{
    [TestFixture]
    public class UnitTests_ProductRepository
    {
        private readonly ILogger<ProductService> _logger;


        [Test]
        public async Task GetAllProductsFromExternalApi_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 10, Description = "This is a product description." },
                new Product { Id = 2, Title = "Product 2", Price = 15, Description = "Another description that is longer than 100 characters and should be shortened." }
            };

            var jsonContent = new JObject(new JProperty("products", JArray.FromObject(expectedProducts))).ToString();

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.GetAllProductsFromExternalApi();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProducts.Count, result.Count());

            for (int i = 0; i < expectedProducts.Count; i++)
            {
                Assert.AreEqual(expectedProducts[i].Id, result.ElementAt(i).Id);
                Assert.AreEqual(expectedProducts[i].Title, result.ElementAt(i).Title);
                Assert.AreEqual(expectedProducts[i].Price, result.ElementAt(i).Price);

                if (expectedProducts[i].Description != null && expectedProducts[i].Description.Length > 100)
                {
                    Assert.AreEqual(100, result.ElementAt(i).Description.Length);
                }
                else
                {
                    Assert.AreEqual(expectedProducts[i].Description, result.ElementAt(i).Description);
                }
            }
        }


        [Test]
        public async Task GetProductByIdFromExternalApi_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product
            {
                Id = productId,
                Title = "Product 1",
                Price = 10,
                Description = "This is a product description."
            };

            var jsonContent = JsonConvert.SerializeObject(expectedProduct);

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.GetProductByIdFromExternalApi(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProduct.Id, result.Id);
            Assert.AreEqual(expectedProduct.Title, result.Title);
            Assert.AreEqual(expectedProduct.Price, result.Price);

            if (expectedProduct.Description != null && expectedProduct.Description.Length > 100)
            {
                Assert.AreEqual(100, result.Description.Length);
            }
            else
            {
                Assert.AreEqual(expectedProduct.Description, result.Description);
            }
        }


        [Test]
        public async Task GetProductsByCategoryAndPriceRangeFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            var category = "electronics";
            var minPrice = 50;
            var maxPrice = 200;

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            var jsonContent = new JObject(new JProperty("products", JArray.FromObject(expectedProducts))).ToString();

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            foreach (var expectedProduct in expectedProducts.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice))
            {
                var actualProduct = result.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.IsNotNull(actualProduct);
                Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
                Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
                Assert.AreEqual(expectedProduct.Category, actualProduct.Category);

                if (expectedProduct.Description != null && expectedProduct.Description.Length > 100)
                {
                    Assert.AreEqual(100, actualProduct.Description.Length);
                }
                else
                {
                    Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
                }
            }
        }


        [Test]
        public async Task GetProductsByCategoryFromExternalApi_ReturnsProducts()
        {
            // Arrange
            var category = "electronics";

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            var jsonContent = new JObject(new JProperty("products", JArray.FromObject(expectedProducts))).ToString();

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.GetProductsByCategoryFromExternalApi(category);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            foreach (var expectedProduct in expectedProducts.Where(p => p.Category == category))
            {
                var actualProduct = result.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.IsNotNull(actualProduct);
                Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
                Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
                Assert.AreEqual(expectedProduct.Category, actualProduct.Category);

                if (expectedProduct.Description != null && expectedProduct.Description.Length > 100)
                {
                    Assert.AreEqual(100, actualProduct.Description.Length);
                }
                else
                {
                    Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
                }
            }
        }

        [Test]
        public async Task GetProductsByPriceRangeFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            var minPrice = 50;
            var maxPrice = 200;

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 240, Description = "Description 3", Category = "beauty" }
            };

            var jsonContent = new JObject(new JProperty("products", JArray.FromObject(expectedProducts))).ToString();

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            foreach (var expectedProduct in expectedProducts.Where(p => p.Price >= minPrice && p.Price <= maxPrice))
            {
                var actualProduct = result.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.IsNotNull(actualProduct);
                Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
                Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
                Assert.AreEqual(expectedProduct.Category, actualProduct.Category);

                if (expectedProduct.Description != null && expectedProduct.Description.Length > 100)
                {
                    Assert.AreEqual(100, actualProduct.Description.Length);
                }
                else
                {
                    Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
                }
            }
        }


        [Test]
        public async Task SearchProductsByNameFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            var searchQuery = "Product";

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Different", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            var jsonContent = new JObject(new JProperty("products", JArray.FromObject(expectedProducts))).ToString();

            // Create a mock HttpMessageHandler
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });

            // Setup HttpClient with the mock message handler
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var productRepository = new ProductRepository(null, httpClient, _logger);

            // Act
            var result = await productRepository.SearchProductsByNameFromExternalApi(searchQuery);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            foreach (var expectedProduct in expectedProducts.Where(p => p.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                var actualProduct = result.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.IsNotNull(actualProduct);
                Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
                Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
                Assert.AreEqual(expectedProduct.Category, actualProduct.Category);

                if (expectedProduct.Description != null && expectedProduct.Description.Length > 100)
                {
                    Assert.AreEqual(100, actualProduct.Description.Length);
                }
                else
                {
                    Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
                }
            }
        }

    }
}
