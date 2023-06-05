using Microsoft.AspNetCore.Authorization;

namespace Mi.Admin.Requirements
{
    public class LoginRequirement: IAuthorizationRequirement
    {
    }

    public class LoginRequirementHandler : AuthorizationHandler<LoginRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }
}
