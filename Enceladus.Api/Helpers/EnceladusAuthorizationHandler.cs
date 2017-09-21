using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{
    internal class EnceladusAuthorizationHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
    // A custom authorization requirement which requires office number to be below a certain value
    internal class ScopeRequirement : IAuthorizationRequirement
    {
        public ScopeRequirement() { }
        
    }
}
