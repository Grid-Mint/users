using FluentValidation;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Queries.EmailExists;

public class EmailExistsQueryHandler(IValidator<EmailExistsQuery> validator, IUserRepository userRepository)
{
    public async Task<Result<bool>> HandleAsync(EmailExistsQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<bool>(new ValidationError(errors));
        }

        var exists = await userRepository.ExistsByEmailAsync(query.Email, cancellationToken);

        return exists;
    }
}
