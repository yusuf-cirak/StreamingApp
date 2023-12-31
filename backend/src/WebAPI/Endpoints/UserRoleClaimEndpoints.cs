using Application.Features.UserRoleClaims.Commands.Create;
using Application.Features.UserRoleClaims.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserRoleClaimEndpoints
{
    public static void MapUserRoleClaimEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/user-role-claims");


        groupBuilder.MapPost("/",
                async ([FromBody] CreateUserRoleClaimCommandRequest createUserRoleClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createUserRoleClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("UserRoleClaims");

        groupBuilder.MapDelete("/",
                async ([FromBody] DeleteUserRoleClaimCommandRequest deleteUserRoleClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(deleteUserRoleClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("UserRoleClaims");
    }
}