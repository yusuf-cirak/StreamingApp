using Microsoft.AspNetCore.Http;

namespace SignalR.Extensions;

public static class HttpContextExtensions
{
    public static bool IsAuthenticated(this HttpContext httpContext) => httpContext?.User is not null;
}