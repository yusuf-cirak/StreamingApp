using Microsoft.AspNetCore.Http;

namespace SharedKernel;

public static partial class HttpResultExtensions
{
    public static IResult ToHttpResponse<TValue>(this HttpResult<TValue> httpResult)
    {
        return httpResult.Match(
            (success, statusCode) => statusCode switch
            {
                200 => Results.Ok(success),
                201 => Results.Created(string.Empty, success),
                204 => Results.NoContent(),
                _ => Results.StatusCode(statusCode)
            },
            (failure, statusCode) => statusCode switch
            {
                400 => Results.BadRequest(failure),
                401 => Results.Unauthorized(),
                403 => Results.Forbid(),
                404 => Results.NotFound(failure),
                409 => Results.Conflict(failure),
                500 => Results.Problem((failure.ToProblemDetails())),
                _ => Results.BadRequest(failure)
            });
    }

    public static IResult ToHttpResponse(this HttpResult httpResult)
    {
        return httpResult.Match(
            (statusCode) => statusCode switch
            {
                200 => Results.Ok(),
                201 => Results.Created(),
                204 => Results.NoContent(),
                _ => Results.StatusCode(statusCode)
            },
            (failure, statusCode) => statusCode switch
            {
                400 => Results.BadRequest(failure),
                401 => Results.Unauthorized(),
                403 => Results.Forbid(),
                404 => Results.NotFound(failure),
                409 => Results.Conflict(failure),
                500 => Results.Problem(failure.ToProblemDetails()),
                _ => Results.BadRequest(failure)
            });
    }
}