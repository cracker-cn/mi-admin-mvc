using System.Security.Claims;

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