using System.ComponentModel.DataAnnotations;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models.Requests;

public class CreateTaskItemRequest
{
    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    public TaskItem ToModel()
    {
        return new TaskItem
        {
            Name = Name
        };
    }
}