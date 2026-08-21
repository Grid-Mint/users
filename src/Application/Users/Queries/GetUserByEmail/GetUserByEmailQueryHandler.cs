using FluentValidation;
using Users.Application.Users.Dtos;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IValidator<GetUserByEmailQuery> validator, IUserRepository userRepository)
{
    public async Task<Result<UserResponse>> HandleAsync(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<UserResponse>(new ValidationError(errors));
        }

        var user = await userRepository.GetByEmailAsync(query.Email, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.Email));

        return new UserResponse(user.Id, user.FirstName, user.LastName, user.FullName, user.Email, user.Role, user.Status, user.CreatedAt, user.UpdatedAt);
    }
}
