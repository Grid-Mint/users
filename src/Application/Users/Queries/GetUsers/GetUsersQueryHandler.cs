using FluentValidation;
using Users.Application.Users.Dtos;
using Users.Domain.Common;
using Users.Domain.Repositories;

namespace Users.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IValidator<GetUsersQuery> validator, IUserRepository userRepository)
{
    public async Task<Result<IReadOnlyList<UserResponse>>> HandleAsync(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<IReadOnlyList<UserResponse>>(new ValidationError(errors));
        }

        var users = await userRepository.GetAllAsync(query.Id, query.Skip, query.Take, cancellationToken);

        var response = users
            .Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.FullName, user.Email, user.Role, user.Status, user.CreatedAt, user.UpdatedAt))
            .ToList();

        return response;
    }
}
