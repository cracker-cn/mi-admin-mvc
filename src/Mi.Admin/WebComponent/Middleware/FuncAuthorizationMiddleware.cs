using System.Security.Claims;

using Mi.Core.Attributes;
using Mi.Core.Enum;
using Mi.Core.Models;
using Mi.Core.Service;
using Mi.Core.Toolkit.Helper;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Mi.Admin.WebComponent.Middleware
{
    public class FuncAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly ILogger<FuncAuthorizationMiddleware> _logger;
        private readonly string[] IGNORE_CONTROLLERS = { "home" };

        public FuncAuthorizationMiddleware(ILogger<FuncAuthorizationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var userModel = context.GetUser();
            var controllerName = (string?)context.Request.RouteValues["controller"] ?? "";

            if (!IGNORE_CONTROLLERS.Contains(controllerName.ToLower()) && userModel != null && userModel.UserId > 0)
            {
                var path = context.Request.Path.Value;
                if (!userModel.IsSuperAdmin)
                {
                    var flag = false;
                    if (userModel.PowerItems != null)
                    {
                        var endpoint = context.GetEndpoint();
                        var attr = endpoint?.Metadata.GetMetadata<AuthorizeCodeAttribute>();
                        if (endpoint != null && attr != null)
                        {
                            flag = userModel.PowerItems!.Any(x => x.AuthCode == attr.Code);
                        }
                    }
                    if (!flag)
                    {
                        await context.Response.WriteAsJsonAsync(new MessageModel(EnumResponseCode.Forbidden, "权限不足，无法访问"));
                        _logger.LogWarning($"'用户Id：{userModel.UserId}，用户名：{userModel.UserName}'访问地址`{path}`权限不足");
                        return;
                    }
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}