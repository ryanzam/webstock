using BlazorWebStock.API.Controllers;
using BlazorWebStock.API.Repositories;
using BlazorWebStockLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlazorWebStock.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _repository = new();

        private ProductController CreateController() => new(_repository.Object);

        [Fact]
        public async Task GetProducts_ReturnsOk_WithProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = "1", ProductName = "Camera", Price = 500m },
                new Product { Id = "2", ProductName = "Phone", Price = 800m }
            };
            _repository.Setup(r => r.GetProducts()).ReturnsAsync(products);

            var result = await CreateController().GetProducts();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(products, ok.Value);
        }

        [Fact]
        public async Task GetProducts_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            _repository.Setup(r => r.GetProducts()).ReturnsAsync((List<Product>)null);

            var result = await CreateController().GetProducts();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetProducts_Returns500_WhenRepositoryThrows()
        {
            _repository.Setup(r => r.GetProducts()).ThrowsAsync(new InvalidOperationException("db down"));

            var result = await CreateController().GetProducts();

            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, error.StatusCode);
        }
    }
}
