using Application.Features.Roles.Commands.Create;
using Application.Features.Roles.Commands.Delete;
using Application.Features.Roles.Queries.GetStreamRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/roles");

        groupBuilder.MapGet("/stream",
                async (
                    IMediator mediator) => (await mediator.Send(new GetStreamRolesQueryRequest())))
            .WithTags("Roles");


        groupBuilder.MapPost("/",
                async ([FromBody] CreateRoleCommandRequest createRoleCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createRoleCommandRequest)).ToHttpResponse();
                })
            .WithTags("Roles");


        groupBuilder.MapDelete("/",
                async ([FromBody] DeleteRoleCommandRequest deleteRoleCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(deleteRoleCommandRequest)).ToHttpResponse();
                })
            .WithTags("Roles");
    }
}