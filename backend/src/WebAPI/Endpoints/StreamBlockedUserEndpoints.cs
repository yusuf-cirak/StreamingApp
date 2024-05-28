using Application.Features.StreamBlockedUsers.Commands.Create;
using Application.Features.StreamBlockedUsers.Commands.Delete;
using Application.Features.StreamBlockedUsers.Queries.GetBlockedUsers;
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

        groupBuilder.MapGet("/all/{streamerId}",
                async (
                    Guid streamerId,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(new GetBlockedUsersFromStreamQueryRequest(streamerId)))
                        .ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");

        groupBuilder.MapGet("/{streamerId}",
                async (
                    Guid streamerId,
                    IMediator mediator) =>
                {
                    return await mediator.Send(new GetIsUserBlockedFromStreamQueryRequest(streamerId));
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


        groupBuilder.MapPut("/",
                async ([FromBody] StreamBlockedUserDeleteCommandRequest streamBlockedUserDeleteCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(streamBlockedUserDeleteCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");
    }
}