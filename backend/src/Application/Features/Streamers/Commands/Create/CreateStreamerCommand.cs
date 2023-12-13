using Application.Abstractions.Repository;
using Application.Abstractions.Security;
using Application.Features.Streamers.Rules;
using Application.Services.Streamers;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Streamers.Commands.Create;

public readonly record struct CreateStreamerCommandRequest(
    Guid UserId) : IRequest<bool>, ISecuredRequest;

public sealed class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommandRequest, bool>
{
    private readonly StreamerBusinessRules _businessRules;
    private readonly IStreamerService _streamerService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEfRepository _efRepository;

    public CreateStreamerCommandHandler(IHttpContextAccessor httpContextAccessor, IEfRepository efRepository,
        StreamerBusinessRules businessRules, IStreamerService streamerService)
    {
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
        _businessRules = businessRules;
        _streamerService = streamerService;
    }

    public async Task<bool> Handle(CreateStreamerCommandRequest request, CancellationToken cancellationToken)
    {
        _businessRules.UserMustBeVerifiedBeforeStreamerAdded(request.UserId);
        
        var user = await _businessRules.UserMustExistBeforeStreamerAdded(request.UserId);
        
        var streamerKey = _streamerService.GenerateStreamerKey(user);

        var stremer = Streamer.Create(user.Id, streamerKey);

        return true;
    }
}