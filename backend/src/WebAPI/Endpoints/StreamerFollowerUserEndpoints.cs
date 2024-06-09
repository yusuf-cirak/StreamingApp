using Application.Features.StreamFollowerUsers.Commands.Create;
using Application.Features.StreamFollowerUsers.Commands.Delete;
using Application.Features.StreamFollowerUsers.Queries.GetFollowersCount;
using Application.Features.StreamFollowerUsers.Queries.GetIsUserFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamerFollowerUserEndpoints
{
    public static void MapStreamerFollowerUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-follower-users");

        groupBuilder.MapGet("/{streamerId}",
                async (Guid streamerId,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(new GetIsUserFollowingStreamQueryRequest(streamerId))).ToHttpResponse();
                })
            .WithTags("StreamerFollowerUsers");


        groupBuilder.MapGet("/count/{streamerId}",
                async (Guid streamerId, IMediator mediator) =>
                {
                    return (await mediator.Send(new GetFollowersCountQueryRequest(streamerId))).ToHttpResponse();
                })
            .WithTags("StreamerFollowerUsers");


        groupBuilder.MapPost("/",
                async ([FromBody] StreamFollowerUserCreateCommandRequest streamFollowerUserCreateCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(streamFollowerUserCreateCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamerFollowerUsers");

        groupBuilder.MapDelete("/",
                async ([FromBody] DeleteStreamFollowerUserCommandRequest deleteStreamFollowerUserCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(deleteStreamFollowerUserCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamerFollowerUsers");
    }
}