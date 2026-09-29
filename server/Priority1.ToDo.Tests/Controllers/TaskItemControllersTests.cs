using Microsoft.AspNetCore.Mvc;
using Priority1.ToDo.Api.Controllers;
using Priority1.ToDo.Core.Domain;
using Priority1.ToDo.Core.Services.Interfaces;
using Moq;

namespace Priority1.ToDo.Tests.Controllers;

public class TaskItemControllersTests
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var service = new Mock<ITaskItemService>();

        service
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Name = "Work"
                }
            });

        var controller = new TaskItemsController(service.Object);

        // Act
        var result = await controller.GetAll(CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenTaskItemDoesNotExist()
    {
        // Arrange
        var service = new Mock<ITaskItemService>();

        service
            .Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var controller = new TaskItemsController(service.Object);

        // Act
        var result = await controller.GetById(
            999,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}