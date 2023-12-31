using Application.Features.RoleOperationClaims.Commands.Create;
using Application.Features.RoleOperationClaims.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class RoleOperationClaimEndpoints
{
    public static void MapRoleOperationClaimEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/role-operation-claims");


        groupBuilder.MapPost("/",
                async ([FromBody] CreateRoleOperationClaimCommandRequest createRoleOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createRoleOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("RoleOperationClaims");


        groupBuilder.MapDelete("/",
                async ([FromBody] DeleteRoleOperationClaimCommandRequest deleteRoleOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(deleteRoleOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("RoleOperationClaims");
    }
}