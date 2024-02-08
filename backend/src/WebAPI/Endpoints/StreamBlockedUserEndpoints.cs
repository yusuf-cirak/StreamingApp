using Application.Features.StreamBlockedUsers.Commands.Create;
using Application.Features.StreamBlockedUsers.Commands.Delete;
using Application.Features.StreamBlockedUsers.Queries.GetIsBlockedFromStream;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamBlockedUserEndpoints
{
    public static void MapStreamBlockedUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-blocked-users");

                groupBuilder.MapGet("/",
                async (
                    [FromBody] GetIsUserBlockedFromStreamQueryRequest getIsUserBlockedFromStreamQueryRequest,
                    IMediator mediator) =>
                {
                    return await mediator.Send(getIsUserBlockedFromStreamQueryRequest);
                })
            .WithTags("StreamBlockedUsers");


        groupBuilder.MapPost("/",
                async (
                    [FromBody] CreateStreamBlockedUserCreateCommandRequest createStreamBlockedUserCreateCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createStreamBlockedUserCreateCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");


        groupBuilder.MapDelete("/",
                async ([FromBody] StreamBlockedUserDeleteCommandRequest streamBlockedUserDeleteCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(streamBlockedUserDeleteCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");
    }
}