using Users.Domain.Common;

namespace Users.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Error error) =>
        Results.Problem(
            title: error.Code,
            detail: error.Message,
            statusCode: error.Type switch
            {
                ErrorType.Validation   => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound     => StatusCodes.Status404NotFound,
                ErrorType.Conflict     => StatusCodes.Status409Conflict,
                _                      => StatusCodes.Status500InternalServerError
            });
}
