using Application.Features.StreamOptions.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamerEndpoints
{
    public static void MapStreamerEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/streamers");


        groupBuilder.MapPatch("/",
                async ([FromBody] UpdateStreamOptionCommandRequest updateStreamOptionCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamOptionCommandRequest)).ToHttpResponse();
                })
            .WithTags("Streamers");
    }
}