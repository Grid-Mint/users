using FluentValidation;

namespace Users.Application.Users.Queries.EmailExists;

public class EmailExistsQueryValidator : AbstractValidator<EmailExistsQuery>
{
    public EmailExistsQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
