using Application.Features.OperationClaims.Commands.Create;
using Application.Features.OperationClaims.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class OperationClaimEndpoints
{
    public static void MapOperationClaimEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/operation-claims");


        groupBuilder.MapPost("/",
                async ([FromBody] CreateOperationClaimCommandRequest createOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("OperationClaims");


        groupBuilder.MapPatch("/",
                async ([FromBody] UpdateOperationClaimCommandRequest updateOperationClaimCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateOperationClaimCommandRequest)).ToHttpResponse();
                })
            .WithTags("OperationClaims");
    }
}