using Application.Features.StreamOptions.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamOptionEndpoints
{
    public static void MapStreamerEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/stream-options");


        groupBuilder.MapPatch("/",
                async ([FromBody] UpdateStreamOptionCommandRequest updateStreamOptionCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamOptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("StreamOptions");
    }
}