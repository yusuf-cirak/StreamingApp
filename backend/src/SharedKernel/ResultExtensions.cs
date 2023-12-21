using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<TValue, TError>(this Result<TValue, TError> result)
        where TError : ProblemDetails
    {
        return result.Match(
            Results.Ok,
            failure => failure.Status switch
            {
                400 => Results.BadRequest(failure),
                401 => Results.Unauthorized(),
                403 => Results.Forbid(),
                404 => Results.NotFound(failure),
                409 => Results.Conflict(failure),
                500 => Results.Problem(failure),
                _ => Results.BadRequest(failure)
            });
    }
}