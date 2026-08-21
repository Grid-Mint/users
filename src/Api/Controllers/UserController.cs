using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Users.Commands.CreateUser;
using Users.Application.Users.Commands.DeleteUser;
using Users.Application.Users.Commands.UpdateUser;
using Users.Application.Users.Commands.UpdateUserEmail;
using Users.Application.Users.Commands.UpdateUserRole;
using Users.Application.Users.Queries.EmailExists;
using Users.Application.Users.Queries.GetUserByEmail;
using Users.Application.Users.Queries.GetUserById;
using Users.Application.Users.Queries.GetUsers;
using Users.Domain.Enums;

namespace Users.Api.Controllers;

[Route("users")]
[ApiController]
public class UserController(
    CreateUserCommandHandler createUserCommandHandler,
    UpdateUserCommandHandler updateUserCommandHandler,
    UpdateUserEmailCommandHandler updateUserEmailCommandHandler,
    UpdateUserRoleCommandHandler updateUserRoleCommandHandler,
    DeleteUserCommandHandler deleteUserCommandHandler,
    EmailExistsQueryHandler emailExistsQueryHandler,
    GetUserByIdQueryHandler getUserByIdQueryHandler,
    GetUserByEmailQueryHandler getUserByEmailQueryHandler,
    GetUsersQueryHandler getUsersQueryHandler) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await createUserCommandHandler.HandleAsync(command, cancellationToken);

        return HandleResult(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] Guid? id, [FromQuery] int skip = 0, [FromQuery] int take = 20, CancellationToken cancellationToken = default)
    {
        var users = await getUsersQueryHandler.HandleAsync(new GetUsersQuery(id, skip, take), cancellationToken);

        return HandleResult(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await getUserByIdQueryHandler.HandleAsync(new GetUserByIdQuery(id), cancellationToken);

        return HandleResult(user);
    }

    [HttpGet("by-email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email, CancellationToken cancellationToken)
    {
        var user = await getUserByEmailQueryHandler.HandleAsync(new GetUserByEmailQuery(email), cancellationToken);

        return HandleResult(user);
    }

    [HttpGet("email-exists")]
    public async Task<IActionResult> EmailExists([FromQuery] string email, CancellationToken cancellationToken)
    {
        var exists = await emailExistsQueryHandler.HandleAsync(new EmailExistsQuery(email), cancellationToken);

        return HandleResult(exists);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await updateUserCommandHandler.HandleAsync(command with { Id = id }, cancellationToken);

        return HandleResult(user);
    }

    [HttpPut("{id:guid}/email")]
    public async Task<IActionResult> UpdateUserEmail(Guid id, [FromBody] UpdateUserEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await updateUserEmailCommandHandler.HandleAsync(command with { Id = id }, cancellationToken);

        return HandleResult(user);
    }

    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] Roles role, CancellationToken cancellationToken)
    {
        var result = await updateUserRoleCommandHandler.HandleAsync(new UpdateUserRoleCommand(id, role), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var result = await deleteUserCommandHandler.HandleAsync(new DeleteUserCommand(id), cancellationToken);

        return HandleResult(result);
    }
}
