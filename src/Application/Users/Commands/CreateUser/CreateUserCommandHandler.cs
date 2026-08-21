using FluentValidation;
using Users.Application.Abstractions;
using Users.Domain.Common;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.Repositories;

namespace Users.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IValidator<CreateUserCommand> validator, IPasswordHasher passwordHasher, IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var hashedPassword = passwordHasher.HashPassword(command.Password);
        var fullName = $"{command.FirstName} {command.LastName}";

        var user = new User{
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            FullName = fullName,
            Email = command.Email,
            PasswordHash = hashedPassword,
            Role = Roles.User,
            CreatedAt = DateTime.UtcNow,
        };


        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
