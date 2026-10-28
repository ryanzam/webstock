using BlazorWebStock.API.Controllers;
using BlazorWebStock.API.Repositories;
using BlazorWebStockLibrary.Model;
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
    }
}
