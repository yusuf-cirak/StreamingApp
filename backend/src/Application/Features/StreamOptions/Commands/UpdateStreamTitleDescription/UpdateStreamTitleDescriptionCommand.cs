using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;
using Application.Features.Streams.Services;

namespace Application.Features.StreamOptions.Commands.Update;

//TODO: Send notification to all current viewers
public readonly record struct UpdateStreamTitleDescriptionCommandRequest
    : IStreamOptionRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public string StreamTitle { get; init; }
    public string StreamDescription { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public UpdateStreamTitleDescriptionCommandRequest()
    {
        AuthorizationFunctions =
            [StreamOptionAuthorizationRules.CanUserGetOrUpdateStreamOptions];
    }

    public UpdateStreamTitleDescriptionCommandRequest(Guid streamerId, string streamTitle, string streamDescription) :
        this()
    {
        StreamerId = streamerId;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
    }
}

public sealed class
    UpdateStreamTitleDescriptionCommandHandler : IRequestHandler<UpdateStreamTitleDescriptionCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStreamService _streamService;

    public UpdateStreamTitleDescriptionCommandHandler(IEfRepository efRepository,
        IHttpContextAccessor httpContextAccessor, IStreamService streamService)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
        _streamService = streamService;
    }

    public async Task<HttpResult> Handle(UpdateStreamTitleDescriptionCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        // var result = await _efRepository.StreamOptions
        //     .Where(st => st.Id == userId)
        //     .ExecuteUpdateAsync(
        //         streamer => streamer
        //             .SetProperty(x => x.StreamTitle, x => request.StreamTitle)
        //             .SetProperty(x => x.StreamDescription, x => request.StreamDescription),
        //         cancellationToken: cancellationToken);


        var streamOptions = await
            _efRepository
                .StreamOptions
                .Include(so => so.Streamer)
                .ThenInclude(user => user.Streams)
                .AsTracking()
                .SingleOrDefaultAsync(so => so.Id == userId,
                    cancellationToken: cancellationToken);


        streamOptions.Update(request.StreamTitle, request.StreamDescription);


        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        _ = _streamService.UpdateStreamOptionCacheAndSendNotificationAsync(streamOptions, cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
    }
}