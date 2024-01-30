using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Mapping;
using Application.Features.Streams.Dtos;
using Application.Features.Streams.Rules;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Features.Streams.Queries.Get;

public readonly record struct GetStreamViewerInfoQueryRequest(string StreamName)
    : IRequest<HttpResult<GetStreamViewerInfoDto>>;

public sealed class
    GetLiveStreamQueryHandler : IRequestHandler<GetStreamViewerInfoQueryRequest, HttpResult<GetStreamViewerInfoDto>>
{
    private readonly IRedisDatabase _redisDatabase;
    private readonly StreamBusinessRules _streamBusinessRules;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetLiveStreamQueryHandler(IRedisDatabase redisDatabase, IHttpContextAccessor httpContextAccessor,
        StreamBusinessRules streamBusinessRules)
    {
        _redisDatabase = redisDatabase;
        _httpContextAccessor = httpContextAccessor;
        _streamBusinessRules = streamBusinessRules;
    }

    public async Task<HttpResult<GetStreamViewerInfoDto>> Handle(GetStreamViewerInfoQueryRequest request,
        CancellationToken cancellationToken)
    {
        var liveStreams = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();


        var liveStreamResult = _streamBusinessRules.IsStreamLive(liveStreams, request.StreamName);

        if (liveStreamResult.IsFailure)
        {
            return liveStreamResult.Error;
        }

        var liveStream = liveStreamResult.Value;


        var viewerUserId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var isUserBlockedFromStream =
            await _streamBusinessRules.IsUserBlockedFromStream(streamerId: liveStream.User.Id, viewerUserId);


        return liveStream.ToDto(isUserBlockedFromStream);
    }
}