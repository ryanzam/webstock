using BlazorWebStock.API.Controllers;
using BlazorWebStock.API.Repositories;
using BlazorWebStockLibrary.Model;
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
    }
}
