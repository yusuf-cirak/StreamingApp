using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<TValue, TError>(this Result<TValue, TError> result)
        where TError : IErrorResponse
    {
        return result.Match(
            Results.Ok,
            failure => failure.StatusCode switch
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

    public static IResult ToHttpResponse<TValue>(this Result<TValue> result)
    {
        return Results.Ok(result);
    }

    public static IResult ToHttpResponse(this Result result)
    {
        return result.Match(
            () => Results.Ok(),
            failure => failure.StatusCode switch
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