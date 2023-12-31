using Application.Features.UserOperationClaims.Commands.Create;
using Application.Features.UserOperationClaims.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserOperationClaimEndpoints
{
    public static void MapUserOperationClaimEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/user-operation-claims");


        groupBuilder.MapPost("/",
                async ([FromBody] CreateUserOperationClaimCommandRequest createUserOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createUserOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("UserOperationClaims");

        groupBuilder.MapDelete("/",
                async ([FromBody] DeleteUserOperationClaimCommandRequest deleteUserOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(deleteUserOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("UserOperationClaims");
    }
}