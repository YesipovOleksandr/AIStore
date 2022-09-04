using AIStore.Domain.Models.Users;
using System.Security.Claims;

namespace AIStore.Domain.Abstract.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User model);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
