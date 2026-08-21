using FluentValidation;

namespace Users.Application.Users.Queries.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0).WithMessage("Skip must not be negative.");

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 100).WithMessage("Take must be between 1 and 100.");
    }
}
