using Application.Features.Streams.Commands.Create;
using Application.Features.Streams.Commands.Update;
using Application.Features.Streams.Queries.Get;
using Application.Features.Streams.Queries.GetAll;
using Application.Features.Streams.Queries.GetViewers;
using Application.Features.Streams.Queries.GetViewersCount;
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

        groupBuilder.MapGet("/live/{streamerName}",
                async (string streamerName, IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamQueryRequest(streamerName))).ToHttpResponse();
                })
            .WithTags("Streams");
        
        groupBuilder.MapGet("/live/viewers-count/{streamerName}",
                async (string streamerName, IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamViewersCountQueryRequest(streamerName))).ToHttpResponse();
                })
            .WithTags("Streams");

        groupBuilder.MapGet("/live/viewers/{streamerName}",
                async (string streamerName, IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamViewersQueryRequest(streamerName))).ToHttpResponse();
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