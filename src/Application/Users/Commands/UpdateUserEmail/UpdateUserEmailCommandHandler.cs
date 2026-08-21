using FluentValidation;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.UpdateUserEmail;

public class UpdateUserEmailCommandHandler(IValidator<UpdateUserEmailCommand> validator, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<Guid>> HandleAsync(UpdateUserEmailCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<Guid>(new ValidationError(errors));
        }

        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        if (!string.Equals(user.Email, command.Email, StringComparison.OrdinalIgnoreCase)
            && await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailAlreadyUsed(command.Email));

        user.Email = command.Email;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
