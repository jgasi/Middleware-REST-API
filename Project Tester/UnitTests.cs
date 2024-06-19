using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Middleware_REST_API.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTester.UnitTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockRepository;
        private IMemoryCache _memoryCache;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IProductRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _productService = new ProductService(_mockRepository.Object, _memoryCache);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCache.Dispose();
        }

        [Test]
        public async Task GetAllProductsFromExternalApi_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 10, Images = new List<string>(), Category = "Category 1", Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 20, Images = new List<string>(), Category = "Category 2", Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllProductsFromExternalApi())
                           .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllProductsFromExternalApi();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProducts.Count, ((List<Product>)result).Count);
        }
    }
}
