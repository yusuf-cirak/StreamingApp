using Application.Common.Constants;
using Application.Features.Streams.Dtos;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Features.Streams.Queries.GetAll;

public readonly record struct GetAllLiveStreamsQueryRequest() : IRequest<HttpResult<List<GetStreamDto>>>;

public sealed class
    GetAllLiveStreamsQueryHandler : IRequestHandler<GetAllLiveStreamsQueryRequest, HttpResult<List<GetStreamDto>>>
{
    private readonly IRedisDatabase _redisDatabase;

    public GetAllLiveStreamsQueryHandler(IRedisDatabase redisDatabase)
    {
        _redisDatabase = redisDatabase;
    }

    public async Task<HttpResult<List<GetStreamDto>>> Handle(GetAllLiveStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var liveStreams = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();

        return liveStreams;
    }
}