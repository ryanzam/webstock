using BlazorWebStock.API.Controllers;
using BlazorWebStock.API.Repositories;
using BlazorWebStockLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlazorWebStock.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _repository = new();

        private CategoryController CreateController() => new(_repository.Object);

        [Fact]
        public async Task GetCategories_ReturnsOk_WithCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Cameras" },
                new Category { Id = "2", Name = "Phones" }
            };
            _repository.Setup(r => r.GetCategories()).ReturnsAsync(categories);

            var result = await CreateController().GetCategories();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(categories, ok.Value);
        }

        [Fact]
        public async Task GetCategories_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            _repository.Setup(r => r.GetCategories()).ReturnsAsync((List<Category>)null);

            var result = await CreateController().GetCategories();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCategories_Returns500_WhenRepositoryThrows()
        {
            _repository.Setup(r => r.GetCategories()).ThrowsAsync(new InvalidOperationException("db down"));

            var result = await CreateController().GetCategories();

            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, error.StatusCode);
        }
    }
}
