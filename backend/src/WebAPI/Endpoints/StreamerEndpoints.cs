using Application.Features.Streamers.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class StreamerEndpoints
{
    public static void MapStreamerEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/streamers");


        groupBuilder.MapPut("/",
                async ([FromBody] UpdateStreamerCommandRequest updateStreamerCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateStreamerCommandRequest)).ToHttpResponse();
                })
            .WithTags("Streamers");
    }
}