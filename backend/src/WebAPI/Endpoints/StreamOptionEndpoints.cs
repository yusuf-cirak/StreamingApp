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

        groupBuilder.MapGet("/chat-settings",
                async ([FromBody] GetStreamChatSettingsQueryRequest getStreamChatSettingsQueryRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(getStreamChatSettingsQueryRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapPatch("/chat-settings",
                async ([FromBody] UpdateStreamChatSettingsCommandRequest updateStreamTitleDescriptionCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamTitleDescriptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapGet("/title-description",
                async ([FromBody] GetStreamTitleDescriptionQueryRequest getStreamTitleDescriptionQueryRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(getStreamTitleDescriptionQueryRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");


        groupBuilder.MapPatch("/title-description",
                async ([FromBody] UpdateStreamTitleDescriptionCommandRequest updateStreamTitleDescriptionCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamTitleDescriptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");
        
        
        
        groupBuilder.MapGet("/key",
                async ([FromBody] GetStreamKeyQueryRequest getStreamKeyQueryRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(getStreamKeyQueryRequest)).ToHttpResponse();
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