using Application.Abstractions;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

    public async Task<User> UserMustExistBeforeStreamerAdded(Guid userId)
    {
        var user = await _efRepository.Users.SingleOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            throw new BusinessException("User does not exist");
        }

        return user;
    }

    public void UserMustBeVerifiedBeforeStreamerAdded(Guid userId)
    {
        if (_httpContextAccessor.HttpContext.User.GetUserId() != userId.ToString())
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }
    }
}