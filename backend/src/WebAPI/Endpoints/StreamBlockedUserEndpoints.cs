﻿using Application.Features.StreamBlockedUsers.Commands.Create;
using Application.Features.StreamBlockedUsers.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamBlockedUserEndpoints
{
    public static void MapStreamBlockedUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-blocked-users");


        groupBuilder.MapPost("/",
                async ([FromBody] StreamBlockedUserCreateCommandRequest createBlockedUserCreateCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createBlockedUserCreateCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");


        groupBuilder.MapDelete("/",
                async (
                    [FromBody] StreamBlockedUserDeleteCommandRequest streamBlockedUserDeleteCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(streamBlockedUserDeleteCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamBlockedUsers");
    }
}