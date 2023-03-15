using System.Security.Claims;

namespace WebApi.Utilities
{
    public static class AuthHelper
    {
        public static bool HasIdClaim(ClaimsPrincipal user)
        {
            return user.Claims.Any(x => x.ValueType == AppClaims.Id);
        }
    }
}
