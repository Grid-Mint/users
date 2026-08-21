namespace Users.Application.Users.Commands.UpdateUserEmail;

public sealed record UpdateUserEmailCommand(
    Guid Id,
    string Email);
