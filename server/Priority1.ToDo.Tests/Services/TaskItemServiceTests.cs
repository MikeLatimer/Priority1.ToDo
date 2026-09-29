using Microsoft.EntityFrameworkCore;
using Priority1.ToDo.Core.Data;
using Priority1.ToDo.Core.Domain;
using Priority1.ToDo.Core.Services;
using Xunit;

namespace Priority1.ToDo.Tests.Services;

public class TaskItemServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsTaskItem()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new TaskItemService(context);

        var taskItem = new TaskItem
        {
            Name = "Work"
        };

        // Act
        var result = await service.CreateAsync(taskItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Work", result.Name);
        Assert.Single(context.TaskItems);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveToFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var taskItem = new TaskItem
        {
            Name = "Work",
            IsActive = true
        };

        context.TaskItems.Add(taskItem);
        await context.SaveChangesAsync();

        var service = new TaskItemService(context);

        // Act
        var result = await service.DeleteAsync(taskItem.Id);

        // Assert
        Assert.True(result);
        Assert.False(taskItem.IsActive);
    }
}