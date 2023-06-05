using System.Security.Claims;

using Mi.Core.Service;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Mi.Admin.WebComponent.Middleware
{
    public class FuncAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var powerService = DotNetService.Get<IPermissionService>();

            var id = long.Parse(context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userName = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (id > 0 && !string.IsNullOrEmpty(userName))
            {
                var userModel = await powerService.QueryUserModelCacheAsync(id, userName);
                if (userModel != null)
                {
                    context.Features.Set(userModel);
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}