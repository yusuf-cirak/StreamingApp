using Application.Common.Mapping;
using Application.Contracts.Streams;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework;

internal static class CompiledQueries
{
    internal static readonly Func<BaseDbContext, IAsyncEnumerable<GetStreamDto>> GetLiveStreamers =
        EF.CompileAsyncQuery((BaseDbContext context) =>
            context.StreamOptions
                .Include(s => s.Streamer)
                .ThenInclude(user => user.Streams)
                .Where(so => so.Streamer.Streams.Any(stream => stream.EndedAt == default))
                .SelectMany(streamOption => streamOption.Streamer.Streams
                    .Take(1)
                    .Select(stream => new
                    {
                        Stream = stream,
                        StreamOption = streamOption
                    }))
                .Select(result =>
                    result.Stream.ToDto(result.StreamOption.Streamer.ToDto(), result.StreamOption.ToDto())));


    internal static readonly Func<BaseDbContext, Guid, IAsyncEnumerable<GetFollowingStreamDto>> GetFollowingStreamers =
        EF.CompileAsyncQuery((BaseDbContext context, Guid userId) =>
            context.StreamFollowerUsers
                .Include(s => s.Streamer)
                .Where(sfu => sfu.UserId == userId)
                .Select(sfu => new GetFollowingStreamDto(sfu.Streamer.ToDto())));
}