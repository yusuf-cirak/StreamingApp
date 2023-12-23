using Application.Abstractions;
using Application.Abstractions.Repository;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Streamers.Rules;

public sealed class StreamerBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StreamerBusinessRules(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }
}