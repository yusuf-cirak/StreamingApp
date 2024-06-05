using Application.Features.Users.Commands.Update;
using Application.Features.Users.Commands.UpdateProfileImage;
using Application.Features.Users.Rules;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapPut("/",
                async ([FromBody] UpdateUserCommandRequest updateUserCommandRequest,
                    IMediator mediator) => (await mediator.Send(updateUserCommandRequest)).ToHttpResponse())
            .WithTags("Users");


        groupBuilder.MapPost("/profile-image",
                async ([FromForm] UpdateProfileImageCommandRequest updateProfileImageCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateProfileImageCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users")
            .DisableAntiforgery();
    }
}