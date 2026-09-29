using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models;

public class TaskItemModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public static TaskItemModel From(TaskItem taskItem)
    {
        return new TaskItemModel
        {
            Id = taskItem.Id,
            Name = taskItem.Name,
            CreateDate = taskItem.CreateDate,
            UpdateDate = taskItem.UpdateDate,
            IsActive = taskItem.IsActive
        };
    }
}