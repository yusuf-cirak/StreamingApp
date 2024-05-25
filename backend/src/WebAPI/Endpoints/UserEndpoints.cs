using Application.Features.Users.Commands.Update;
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
                    IMediator mediator) =>
                {
                    return (await mediator.Send(updateUserCommandRequest)).ToHttpResponse();
                })
            .WithTags("Users");
    }
}