using Mi.Core.Service;
using Mi.IService.System;

namespace Mi.Admin.WebComponent.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isLogin = context.User.Identity?.IsAuthenticated ?? false;
            var data = context.GetUserData();
            if (isLogin && !string.IsNullOrWhiteSpace(data))
            {
                var permissionService = DotNetService.Get<IPermissionService>();
                var userModel = await permissionService.QueryUserModelCacheAsync(data);
                context.Features.Set(userModel);
            }

            await _next(context);
        }
    }
}