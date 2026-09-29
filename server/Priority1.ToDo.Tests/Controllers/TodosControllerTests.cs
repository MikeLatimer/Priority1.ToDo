using Microsoft.AspNetCore.Mvc;
using Moq;
using Priority1.ToDo.Api.Controllers;
using Priority1.ToDo.Core.Domain;
using Priority1.ToDo.Core.Services.Interfaces;
using Xunit;

namespace Priority1.ToDo.Tests.Controllers;

public class TodosControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var service = new Mock<ITodoService>();

        service
            .Setup(x => x.GetAllAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Todo>
            {
                new Todo
                {
                    Id = 1,
                    Title = "Test Todo",
                    TaskItemId = 1
                }
            });

        var controller = new TodosController(service.Object);

        // Act
        var result = await controller.GetAll(
            1,
            CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var service = new Mock<ITodoService>();

        service
            .Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Todo?)null);

        var controller = new TodosController(service.Object);

        // Act
        var result = await controller.GetById(
            999,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}