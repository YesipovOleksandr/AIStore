using AIStore.Domain.Enums;
using System.Security.Claims;

namespace AIStore.Domain.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static long? GetId(this ClaimsPrincipal claimsPrincipal)
        {
            long.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long userId);

            if (userId <= 0)
                return null;

            return userId;
        }

        public static string? GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static Role GetRole(this ClaimsPrincipal claimsPrincipal)
        {
            Enum.TryParse(claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value, out Role role);
            return role;
        }
    }
}

