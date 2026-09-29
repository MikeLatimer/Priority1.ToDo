using FluentValidation;
using Priority1.ToDo.Api.Models.Requests;

namespace Priority1.ToDo.Api.Validators;

public class UpdateTaskItemRequestValidator : AbstractValidator<UpdateTaskItemRequest>
{
    public UpdateTaskItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150)
            .WithMessage("Name cannot exceed 150 characters.");
    }
}