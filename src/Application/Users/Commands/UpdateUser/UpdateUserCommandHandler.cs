using FluentValidation;
using Users.Application.Abstractions;
using Users.Domain.Common;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IValidator<UpdateUserCommand> validator, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<Guid>> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<Guid>(new ValidationError(errors));
        }

        var fullName = $"{command.FirstName} {command.LastName}";
        
        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.FullName = fullName;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
