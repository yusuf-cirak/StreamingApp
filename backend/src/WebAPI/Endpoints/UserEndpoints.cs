using Application.Features.Auths.Commands.Login;
using Application.Features.Auths.Commands.Refresh;
using Application.Features.Auths.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapPost("/register",
                async (RegisterCommandRequest registerCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(registerCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users");


        groupBuilder.MapPost("/login",
                async (LoginCommandRequest loginCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(loginCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users");

        groupBuilder.MapPost("/refresh",
                async ([FromBody] RefreshTokenCommandRequest refreshTokenCommandRequest, IMediator mediator) =>
                {
                    return (await mediator.Send(refreshTokenCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users");
    }
}