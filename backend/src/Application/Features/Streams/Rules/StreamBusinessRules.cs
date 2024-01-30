using Application.Abstractions;
using Application.Features.Streams.Dtos;

namespace Application.Features.Streams.Rules;

public sealed class StreamBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public StreamBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public Result<GetStreamDto, Error> IsStreamLive(List<GetStreamDto> streams, Guid streamerId)
    {
        var stream = streams.SingleOrDefault(ls => ls.User.Id == streamerId);
        if (stream is null)
        {
            return (StreamErrors.StreamIsNotLive);
        }

        return stream;
    }

    public Result<GetStreamDto, Error> IsStreamLive(List<GetStreamDto> streams, string username)
    {
        var stream = streams.SingleOrDefault(ls => ls.User.Username == username);
        if (stream is null)
        {
            return (StreamErrors.StreamIsNotLive);
        }

        return stream;
    }

    public async Task<bool> IsUserBlockedFromStream(Guid streamerId, string viewerUserId)
    {
        if (viewerUserId == String.Empty)
        {
            return false;
        }

        return await _efRepository.StreamBlockedUsers.AnyAsync(sbu =>
            sbu.StreamerId == streamerId && sbu.UserId == Guid.Parse(viewerUserId));
    }
}