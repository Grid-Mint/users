using FluentValidation;
using Users.Application.Abstractions;
using Users.Domain.Common;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IValidator<CreateUserCommand> validator, IPasswordHasher passwordHasher, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<Guid>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, ErrorType.Validation))
                .ToArray();

            return Result.Failure<Guid>(new ValidationError(errors));
        }

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return UserErrors.EmailAlreadyUsed(command.Email);

        var hashedPassword = passwordHasher.HashPassword(command.Password);
        var fullName = $"{command.FirstName} {command.LastName}";
        var id = Guid.NewGuid();

        var user = new User{
            Id = id, 
            FirstName = command.FirstName,
            LastName = command.LastName,
            FullName = fullName,
            Email = command.Email,
            PasswordHash = hashedPassword,
            Role = Roles.User,
            CreatedAt = DateTime.UtcNow,
            Status = Statuses.Active
        };

        var createdUser = await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return createdUser.Id;
    }
}
