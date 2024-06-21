using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middleware_REST_API.Controllers;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;
using Moq;

namespace Project_Tester.UnitTests
{
    [TestFixture]
    public class UnitTests_ProductController
    {
        private Mock<IProductService> _mockProductService;
        private ProductsController _controller;
        private readonly ILogger<ProductService> _logger;


        [SetUp]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object, _logger);
        }

        [Test]
        public async Task GetAllProductsFromExternalApi_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" }
            };

            _mockProductService.Setup(service => service.GetAllProductsFromExternalApi())
                               .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetAllProductsFromExternalApi();

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProducts = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(expectedProducts.Count, actualProducts.Count());
        }


        [Test]
        public async Task GetProductByIdFromExternalApi_ExistingId_ReturnsProduct()
        {
            // Arrange
            int existingProductId = 1;
            var expectedProduct = new Product { Id = existingProductId, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" };

            _mockProductService.Setup(service => service.GetProductByIdFromExternalApi(existingProductId))
                               .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProductByIdFromExternalApi(existingProductId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProduct = okResult.Value as Product;
            Assert.IsNotNull(actualProduct);
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
            Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
            Assert.AreEqual(expectedProduct.Category, actualProduct.Category);
        }

        [Test]
        public async Task GetProductByIdFromExternalApi_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingProductId = 999;
            _mockProductService.Setup(service => service.GetProductByIdFromExternalApi(nonExistingProductId))
                               .ThrowsAsync(new ProductNotFoundException($"Product with ID {nonExistingProductId} not found"));

            // Act
            var result = await _controller.GetProductByIdFromExternalApi(nonExistingProductId);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetProductsByCategoryAndPriceRangeFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            string category = "electronics";
            decimal minPrice = 50;
            decimal maxPrice = 200;

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            _mockProductService.Setup(service => service.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice))
                               .ReturnsAsync(expectedProducts.Where(p => p.Category == category));

            // Act
            var result = await _controller.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProducts = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(2, actualProducts.Count());
        }


        [Test]
        public async Task GetProductsByCategoryFromExternalApi_ReturnsProducts()
        {
            // Arrange
            string category = "electronics";

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            _mockProductService.Setup(service => service.GetProductsByCategoryFromExternalApi(category))
                               .ReturnsAsync(expectedProducts.Where(p => p.Category == category));

            // Act
            var result = await _controller.GetProductsByCategoryFromExternalApi(category);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProducts = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(2, actualProducts.Count());
        }

        [Test]
        public async Task GetProductsByPriceRangeFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            decimal minPrice = 50;
            decimal maxPrice = 200;

            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Product 3", Price = 240, Description = "Description 3", Category = "beauty" }
            };


            _mockProductService.Setup(service => service.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice))
                               .ReturnsAsync(expectedProducts.Where(p => p.Price >= minPrice && p.Price <= maxPrice));

            // Act
            var result = await _controller.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProducts = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(2, actualProducts.Count());
        }

        [Test]
        public async Task SearchProductsByNameFromExternalApi_ReturnsFilteredProducts()
        {
            // Arrange
            string name = "Product";
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 100, Description = "Description 1", Category = "electronics" },
                new Product { Id = 2, Title = "Product 2", Price = 150, Description = "Description 2", Category = "electronics" },
                new Product { Id = 3, Title = "Different", Price = 140, Description = "Description 3", Category = "beauty" }
            };

            _mockProductService.Setup(service => service.SearchProductsByNameFromExternalApi(name))
                   .ReturnsAsync(expectedProducts.Where(p => p.Title.Contains(name)).ToList());

            // Act
            var result = await _controller.SearchProductsByNameFromExternalApi(name);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualProducts = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(2, actualProducts.Count());
        }
    }
}

