using WebAPI.Endpoints;

namespace WebAPI.Extensions;

public static class EndpointExtensions
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapOperationClaimEndpoints();
        app.MapStreamBlockedUserEndpoints();
        app.MapRoleOperationClaimEndpoints();
        app.MapRoleEndpoints();
        app.MapStreamerEndpoints();
        app.MapStreamerFollowerUserEndpoints();
        app.MapStreamEndpoints();
        app.MapUserOperationClaimEndpoints();
        app.MapUserRoleClaimEndpoints();
        app.MapUserEndpoints();
    }
}