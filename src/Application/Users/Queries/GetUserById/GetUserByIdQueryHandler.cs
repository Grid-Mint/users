using FluentValidation;
using Users.Application.Users.Dtos;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IValidator<GetUserByIdQuery> validator, IUserRepository userRepository)
{
    public async Task<Result<UserResponse>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<UserResponse>(new ValidationError(errors));
        }

        var user = await userRepository.GetByIdAsync(query.Id, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.Id));

        return new UserResponse(user.Id, user.FirstName, user.LastName, user.FullName, user.Email, user.Role, user.Status, user.CreatedAt, user.UpdatedAt);
    }
}
