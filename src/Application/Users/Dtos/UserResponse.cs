using Users.Domain.Enums;

namespace Users.Application.Users.Dtos;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? FullName,
    string Email,
    Roles Role,
    Statuses Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
