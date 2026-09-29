using Microsoft.EntityFrameworkCore;
using Priority1.ToDo.Core.Data;
using Priority1.ToDo.Core.Domain;
using Priority1.ToDo.Core.Services;
using Xunit;

namespace Priority1.ToDo.Tests.Services;

public class TodoServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsTodo()
    {
        // Arrange
        await using var context = CreateContext();

        var taskItem = new TaskItem
        {
            Name = "Work"
        };

        context.TaskItems.Add(taskItem);
        await context.SaveChangesAsync();

        var service = new TodoService(context);

        var todo = new Todo
        {
            Title = "Finish project",
            TaskItemId = taskItem.Id,
            DueDate = DateTime.Today.AddDays(1)
        };

        // Act
        var result = await service.CreateAsync(todo);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Finish project", result.Title);
        Assert.Equal(taskItem.Id, result.TaskItemId);
        Assert.Single(context.Todos);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyTodosForSelectedTaskItem()
    {
        // Arrange
        await using var context = CreateContext();

        var firstList = new TaskItem
        {
            Name = "Work"
        };

        var secondList = new TaskItem
        {
            Name = "Home"
        };

        context.TaskItems.AddRange(firstList, secondList);
        await context.SaveChangesAsync();

        context.Todos.AddRange(
            new Todo
            {
                Title = "Work Todo",
                TaskItemId = firstList.Id,
                IsActive = true
            },
            new Todo
            {
                Title = "Home Todo",
                TaskItemId = secondList.Id,
                IsActive = true
            }
        );

        await context.SaveChangesAsync();

        var service = new TodoService(context);

        // Act
        var result = await service.GetAllAsync(firstList.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal("Work Todo", result[0].Title);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveToFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var taskItem = new TaskItem
        {
            Name = "Work"
        };

        context.TaskItems.Add(taskItem);
        await context.SaveChangesAsync();

        var todo = new Todo
        {
            Title = "Test Todo",
            TaskItemId = taskItem.Id,
            IsActive = true
        };

        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        var service = new TodoService(context);

        // Act
        var result = await service.DeleteAsync(todo.Id);

        // Assert
        Assert.True(result);
        Assert.False(todo.IsActive);
    }
}