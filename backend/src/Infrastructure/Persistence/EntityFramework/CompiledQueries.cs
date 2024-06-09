using Application.Common.Mapping;
using Application.Contracts.Streams;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework;

internal static class CompiledQueries
{
    internal static readonly Func<BaseDbContext, IAsyncEnumerable<GetStreamDto>> GetLiveStreamers =
        EF.CompileAsyncQuery((BaseDbContext context) =>
            context
                .Streams
                .AsTracking()
                .Include(s => s.Streamer.Streams)
                .Include(s => s.Streamer.StreamOption)
                .Where(s => s.EndedAt == default)
                .Select(stream =>
                    stream.ToDto(stream.Streamer.ToDto(), stream.Streamer.StreamOption.ToDto(false, null))));
}