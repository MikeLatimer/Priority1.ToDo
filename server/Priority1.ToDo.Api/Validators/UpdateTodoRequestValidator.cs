using FluentValidation;
using Priority1.ToDo.Api.Models.Requests;

namespace Priority1.ToDo.Api.Validators;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.TaskItemId)
            .GreaterThan(0)
            .WithMessage("TaskItemId must be greater than 0.");

        RuleFor(x => x.DueDate)
           .Must(dueDate => !dueDate.HasValue || dueDate.Value.Date >= DateTime.Today)
           .WithMessage("Due date cannot be in the past.");
    }
}