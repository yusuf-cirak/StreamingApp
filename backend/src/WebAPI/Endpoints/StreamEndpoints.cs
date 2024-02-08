using Application.Features.Streams.Commands.Create;
using Application.Features.Streams.Commands.Update;
using Application.Features.Streams.Queries.GetAll;
using Application.Features.Streams.Queries.GetFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamEndpoints
{
    public static void MapStreamEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/streams");

        groupBuilder.MapGet("/live",
                async (IMediator mediator) =>
                {
                    return (await mediator.Send(new GetAllLiveStreamsQueryRequest())).ToHttpResponse();
                })
            .WithTags("Streams");

        groupBuilder.MapGet("/following",
                async (IMediator mediator) =>
                {
                    return (await mediator.Send(new GetFollowingStreamsQueryRequest())).ToHttpResponse();
                })
            .WithTags("Streams");

        groupBuilder.MapPost("/",
                async ([FromBody] CreateStreamCommandRequest createStreamCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(createStreamCommandRequest)).ToHttpResponse();
                })
            .WithTags("Streams");


        groupBuilder.MapPatch("/",
                async ([FromBody] UpdateStreamCommandRequest updateStreamCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamCommandRequest)).ToHttpResponse();
                })
            .WithTags("Streams");
    }
}