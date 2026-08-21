using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Users.Commands.CreateUser;

namespace Users.Api.Controllers;

[Route("users/")]
[ApiController]
public class UserController(CreateUserCommandHandler createUserCommandHandler) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await createUserCommandHandler.HandleAsync(command, cancellationToken);

        return HandleResult(user);
    }
}
