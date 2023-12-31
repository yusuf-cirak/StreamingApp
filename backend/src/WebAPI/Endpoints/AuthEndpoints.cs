using Application.Features.Auths.Commands.Login;
using Application.Features.Auths.Commands.Refresh;
using Application.Features.Auths.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/auths");


        groupBuilder.MapPost("/register",
                async (RegisterCommandRequest registerCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(registerCommandRequest)).ToHttpResponse();
                })
            .WithTags("Auths");


        groupBuilder.MapPost("/login",
                async (LoginCommandRequest loginCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(loginCommandRequest)).ToHttpResponse();
                })
            .WithTags("Auths");

        groupBuilder.MapPost("/refresh",
                async ([FromBody] RefreshTokenCommandRequest refreshTokenCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(refreshTokenCommandRequest)).ToHttpResponse();
                })
            .WithTags("Auths");
    }
}