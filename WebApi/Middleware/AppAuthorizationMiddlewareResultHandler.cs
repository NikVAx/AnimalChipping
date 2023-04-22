using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using WebApi.Requirements;

namespace WebApi.Middleware
{
    public class AppAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defatultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {

            if(authorizeResult.Forbidden
                && authorizeResult.AuthorizationFailure!.FailedRequirements
                .OfType<IdentifiedRequirement>().Any())
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await defatultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
