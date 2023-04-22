using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Requirements;

namespace WebApi.Handlers
{
    public class IdentifiedHandler : AuthorizationHandler<IdentifiedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IdentifiedRequirement requirement)
        {
            var hasIdClaim = context.User.Claims.Any(x => x.Type == AppClaims.Id);

            if(hasIdClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
