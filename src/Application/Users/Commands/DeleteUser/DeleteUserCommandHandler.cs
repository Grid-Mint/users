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

        var deleted = await userRepository.SoftDeleteAsync(command.Id, cancellationToken);

        if (deleted)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        var existing = await userRepository.GetAnyByIdAsync(command.Id, cancellationToken);

        return existing is null
            ? Result.Failure(UserErrors.NotFound(command.Id))
            : Result.Failure(UserErrors.AlreadyDeleted(command.Id));
    }
}
