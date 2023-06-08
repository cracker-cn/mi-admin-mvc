using System.Security.Claims;

using Mi.Core.Attributes;
using Mi.Core.Enum;
using Mi.Core.Models;
using Mi.Core.Service;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Mi.Admin.WebComponent.Middleware
{
    public class FuncAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly ILogger<FuncAuthorizationMiddleware> _logger;

        public FuncAuthorizationMiddleware(ILogger<FuncAuthorizationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var powerService = DotNetService.Get<IPermissionService>();

            var id = long.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userName = context.User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
            if (id > 0 && !string.IsNullOrEmpty(userName))
            {
                var userModel = await powerService.QueryUserModelCacheAsync(id, userName);
                if (userModel != null)
                {
                    var controllerName = (string?)context.Request.RouteValues["controller"];
                    var actionName = (string?)context.Request.RouteValues["action"];
                    var path = context.Request.Path.Value;
                    var flag = controllerName != null && controllerName.ToLower() == "home";
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
                        _logger.LogWarning($"'{userModel.UserName}'访问地址'{path}'权限不足");
                        return;
                    }
                    context.Features.Set(userModel);
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}