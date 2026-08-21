using FluentValidation;

namespace Users.Application.Users.Commands.UpdateUserEmail;

public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
