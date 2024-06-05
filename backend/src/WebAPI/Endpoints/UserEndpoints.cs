using Application.Features.Users.Commands.DeleteStreamModerator;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Commands.UpdateProfileImage;
using Application.Features.Users.Commands.UpsertStreamModerator;
using Application.Features.Users.Queries.GetByTerm;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("api/users");


        groupBuilder.MapGet("/search",
                async ([FromQuery] string term,
                    IMediator mediator) => (await mediator.Send(new GetUsersByTermQueryRequest(term))))
            .WithTags("Users");


        groupBuilder.MapPost("/profile-image",
                async ([FromForm] UpdateProfileImageCommandRequest updateProfileImageCommandRequest,
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateProfileImageCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users")
            .DisableAntiforgery();

        groupBuilder.MapPost("/stream-moderators",
                async ([FromBody] UpsertStreamModeratorsCommandRequest userCreateStreamPermissionCommandRequest,
                        IMediator mediator) =>
                    (await mediator.Send(userCreateStreamPermissionCommandRequest)).ToHttpResponse())
            .WithTags("Users");

        groupBuilder.MapPut("/",
                async ([FromBody] UpdateUserCommandRequest updateUserCommandRequest,
                    IMediator mediator) => (await mediator.Send(updateUserCommandRequest)).ToHttpResponse())
            .WithTags("Users");
        
        groupBuilder.MapDelete("/stream-moderators",
                async ([FromBody] DeleteStreamModeratorsCommandRequest userCreateStreamPermissionCommandRequest,
                        IMediator mediator) =>
                    (await mediator.Send(userCreateStreamPermissionCommandRequest)).ToHttpResponse())
            .WithTags("Users");
    }
}