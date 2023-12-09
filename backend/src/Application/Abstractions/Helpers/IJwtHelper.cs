using Application.Common.Models;

namespace Application.Abstractions.Helpers;

public interface IJwtHelper
{
    AccessToken CreateAccessToken(User user);
    RefreshToken CreateRefreshToken(User user, string ipAddress);

}