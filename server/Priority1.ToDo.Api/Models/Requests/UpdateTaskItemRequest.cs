using System.ComponentModel.DataAnnotations;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models.Requests;

public class UpdateTaskItemRequest
{
    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    public TaskItem ToModel(int id)
    {
        return new TaskItem
        {
            Id = id,
            Name = Name
        };
    }
}