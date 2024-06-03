using Application.Contracts.Common.Models;

namespace Application.Abstractions.Helpers;

public interface IJwtHelper
{
    AccessToken CreateAccessToken(User user, Dictionary<string, object> claimsDictionary);
    RefreshToken CreateRefreshToken(User user, string ipAddress);
}