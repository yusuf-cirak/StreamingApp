using Application.Features.Auths.Commands.Login;
using Application.Features.Auths.Commands.Refresh;
using Application.Features.Auths.Commands.Register;
using MediatR;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapPost("/register",
                async (RegisterCommandRequest registerCommandRequest, IMediator mediator) =>
                {
                    return await mediator.Send(registerCommandRequest);
                })
            .WithTags("Users");


        groupBuilder.MapPost("/login",
                async (LoginCommandRequest loginCommandRequest, IMediator mediator) =>
                {
                    return await mediator.Send(loginCommandRequest);
                })
            .WithTags("Users");

        groupBuilder.MapPost("/refresh",
                async (RefreshTokenCommandRequest refreshTokenCommandRequest, IMediator mediator) =>
                {
                    return await mediator.Send(refreshTokenCommandRequest);
                })
            .WithTags("Users");
    }
}