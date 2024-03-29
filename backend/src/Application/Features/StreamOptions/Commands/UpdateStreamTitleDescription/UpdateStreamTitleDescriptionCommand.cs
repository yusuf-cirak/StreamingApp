﻿using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Commands.Update;

// TODO: Update chat disabled, delay
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

    public UpdateStreamTitleDescriptionCommandRequest(Guid streamerId, string streamTitle, string streamDescription) : this()
    {
        StreamerId = streamerId;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
    }
}

public sealed class UpdateStreamTitleDescriptionCommandHandler : IRequestHandler<UpdateStreamTitleDescriptionCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateStreamTitleDescriptionCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult> Handle(UpdateStreamTitleDescriptionCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        var result = await _efRepository.StreamOptions
            .Where(st => st.Id == userId)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamTitle, x => request.StreamTitle)
                    .SetProperty(x => x.StreamDescription, x => request.StreamDescription),
                cancellationToken: cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
    }
}