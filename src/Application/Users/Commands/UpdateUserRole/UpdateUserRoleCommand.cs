using Users.Domain.Enums;

namespace Users.Application.Users.Commands.UpdateUserRole;

public sealed record UpdateUserRoleCommand(Guid Id, Roles Role);
