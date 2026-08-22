using Users.Domain.Common;

namespace Users.Domain.Errors;

public static class UserErrors
{
    public static Error DeleteFailed(Guid id) => new(
        "Users.DeleteFailed",
        $"Failed to delete user with id '{id}'.",
        ErrorType.Conflict);

    public static Error NotFound(Guid id) => new(
        "Users.NotFound",
        $"User with id '{id}' was not found.",
        ErrorType.NotFound);

    public static Error NotFound(string email) => new(
        "Users.NotFound",
        $"User with email '{email}' was not found.",
        ErrorType.NotFound);

    public static Error EmailAlreadyUsed(string email) => new(
        "Users.EmailAlreadyUsed",
        $"Email '{email}' is already in use.",
        ErrorType.Conflict);

    public static readonly Error InvalidCredentials = new(
        "Users.InvalidCredentials",
        "The provided credentials are incorrect.",
        ErrorType.Unauthorized);

    public static readonly Error NotActive = new(
        "Users.NotActive",
        "The account is not active.",
        ErrorType.Unauthorized);

    public static Error AlreadyDeleted(Guid id) => new(
        "Users.AlreadyDeleted",
        $"User '{id}' is already deleted.",
        ErrorType.Conflict);

    public static readonly Error CannotModifySelf = new(
        "Users.CannotModifySelf",
        "You cannot perform this action on your own account.",
        ErrorType.Validation);

    public static readonly Error InsufficientRole = new(
        "Users.InsufficientRole",
        "You do not have permission to assign this role.",
        ErrorType.Unauthorized);
}