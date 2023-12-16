using Application.Abstractions.Repository;
using Application.Abstractions.Security;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Streamers.Commands.Update;

public readonly record struct UpdateStreamerCommandRequest(string StreamTitle, string StreamDescription)
    : IRequest<bool>, ISecuredRequest;

public sealed class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommandRequest, bool>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateStreamerCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(UpdateStreamerCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());


        await _efRepository.Streamers
            .Where(st => st.Id == userId)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamTitle, x => request.StreamTitle)
                    .SetProperty(x => x.StreamDescription, x => request.StreamDescription),
                cancellationToken: cancellationToken);

        return true;
    }
}