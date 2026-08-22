using FluentValidation;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IValidator<DeleteUserCommand> validator, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure(new ValidationError(errors));
        }

        var isUserDeleted = await userRepository.GetDeletedByIdAsync(command.Id, cancellationToken);

        if (isUserDeleted is not null)
            return Result.Failure(UserErrors.AlreadyDeleted(command.Id));

        var found = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (found is null)
            return Result.Failure(UserErrors.NotFound(command.Id));

        var deleted = await userRepository.SoftDeleteAsync(command.Id, cancellationToken);

        if (!deleted)
            return Result.Failure(UserErrors.DeleteFailed(command.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
