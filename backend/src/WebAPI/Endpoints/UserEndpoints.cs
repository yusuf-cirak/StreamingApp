using Application.Features.Auths.Commands.Register;
using Application.Features.Auths.Dtos;
using MediatR;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapPost("/register", async (RegisterCommandRequest registerCommandRequest, IMediator mediator) =>
        {
            return await mediator.Send(registerCommandRequest);
        })
        .WithTags("Users");
    }

}