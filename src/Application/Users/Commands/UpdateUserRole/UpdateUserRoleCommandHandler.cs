using FluentValidation;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommandHandler(IValidator<UpdateUserRoleCommand> validator, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result> HandleAsync(UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure(new ValidationError(errors));
        }

        var updated = await userRepository.UpdateRoleAsync(command.Id, command.Role, cancellationToken);

        if (!updated)
            return Result.Failure(UserErrors.NotFound(command.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
