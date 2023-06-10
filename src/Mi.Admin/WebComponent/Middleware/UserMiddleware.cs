using System.Security.Claims;

using Mi.Core.Service;
using Mi.Core.Toolkit.Helper;
using Mi.IService.System;

namespace Mi.Admin.WebComponent.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserMiddleware> _logger;
        private readonly string[] IGNORE_CONTROLLERS = { "home", "public", "personal" };

        public UserMiddleware(RequestDelegate next, ILogger<UserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isLogin = context.User.Identity?.IsAuthenticated ?? false;
            var data = context.GetUserData();
            if (isLogin && !string.IsNullOrWhiteSpace(data))
            {
                var permissionService = DotNetService.Get<IPermissionService>();
                var userModel = await permissionService.QueryUserModelAsync(data);

                context.Features.Set(userModel);
            }

            await _next(context);
        }
    }

    public static class UserMiddlewareExtension
    {
        public static IApplicationBuilder UseFetchUser(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserMiddleware>();
        }
    }
}