namespace Application.Features.StreamBlockedUsers.Rules;

public sealed class StreamBlockedUserBusinessRules
{
    private readonly IEfRepository _efRepository;

    public StreamBlockedUserBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }
    
}