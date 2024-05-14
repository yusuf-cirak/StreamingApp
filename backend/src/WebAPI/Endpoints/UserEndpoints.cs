using Application.Features.Users.Commands.Update;
using Application.Features.Users.Queries.GetBlocked;
using Application.Features.Users.Queries.GetFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapGet("/following-streamers",
                async (IMediator mediator) =>
                {
                    return (await mediator.Send(new GetFollowingStreamsQueryRequest())).ToHttpResponse();
                })
            .WithTags("Users");

        groupBuilder.MapGet("/blocked-streamers",
                async (IMediator mediator) =>
                {
                    return (await mediator.Send(new GetBlockedStreamsQueryRequest())).ToHttpResponse();
                })
            .WithTags("Users");

        groupBuilder.MapPut("/",
                async ([FromBody] UpdateUserCommandRequest updateUserCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateUserCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users");
    }
}