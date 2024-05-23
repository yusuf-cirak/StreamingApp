using Application.Features.StreamOptions.Commands.Update;
using Application.Features.StreamOptions.Queries.GetChatSettings;
using Application.Features.StreamOptions.Queries.GetStreamKey;
using Application.Features.StreamOptions.Queries.GetStreamTitleDescription;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamOptionEndpoints
{
    public static void MapStreamerEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-options");

        groupBuilder.MapGet("/chat-settings/{streamerId}",
                async (Guid streamerId,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamChatSettingsQueryRequest(streamerId))).ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapPatch("/chat-settings",
                async ([FromBody] UpdateStreamChatSettingsCommandRequest updateStreamTitleDescriptionCommandRequest,
                    IMediator mediator) =>
                {
                    System.Console.WriteLine(updateStreamTitleDescriptionCommandRequest);
                    return (await mediator.Send(updateStreamTitleDescriptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapGet("/title-description/{streamerId}",
                async (Guid streamerId,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamTitleDescriptionQueryRequest(streamerId)))
                        .ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapPatch("/title-description",
                async ([FromForm] UpdateStreamTitleDescriptionCommandRequest updateStreamTitleDescriptionCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamTitleDescriptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions")
            .DisableAntiforgery();


        groupBuilder.MapGet("/key/{streamerId}",
                async (Guid streamerId, IMediator mediator) =>
                {
                    return (await mediator.Send(new GetStreamKeyQueryRequest(streamerId))).ToHttpResponse();
                })
            .WithTags("StreamOptions");

        groupBuilder.MapPost("/key",
                async ([FromBody] GenerateStreamKeyCommandRequest generateStreamKeyCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(generateStreamKeyCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");
    }
}