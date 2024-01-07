using Application.Abstractions;
using Application.Features.Streams.Dtos;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Rules;

public sealed class StreamBusinessRules : BaseBusinessRules
{
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
    
}