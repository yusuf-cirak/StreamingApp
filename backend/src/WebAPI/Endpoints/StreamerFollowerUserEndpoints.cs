﻿using Application.Features.StreamFollowerUsers.Commands.Create;
using Application.Features.StreamFollowerUsers.Commands.Delete;
using Application.Features.StreamFollowerUsers.Queries.GetIsUserFollowingStream;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamerFollowerUserEndpoints
{
    public static void MapStreamerFollowerUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-follower-users");

                groupBuilder.MapGet("/",
                async ([FromBody] GetIsUserFollowingStreamQueryRequest streamFollowerUserCreateCommandRequest,
                    IMediator mediator) =>
                {
                    return await mediator.Send(streamFollowerUserCreateCommandRequest);
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