using System.Text;

using Mi.Admin.Areas.Account.Controllers;
using Mi.Admin.Controllers.BASE;
using Mi.Core.CommonOption;
using Mi.Core.Models;
using Mi.IService.System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Admin.WebComponent.Filter
{
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<GlobalActionFilterAttribute> _logger;
        private readonly ILogService _logService;
        private readonly static string?[] IGNORE_CONTROLLERS = new string?[2] { typeof(PublicController).FullName,
            typeof(LoginController).FullName };

        public GlobalActionFilterAttribute(ILogger<GlobalActionFilterAttribute> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys.Where(key => context.ModelState[key] != null).Select(key => new Option { Name = key, Value = context.ModelState[key]!.Errors.FirstOrDefault()?.ErrorMessage });
                if (result != null)
                {
                    var msg = result.Where(x => !string.IsNullOrEmpty(x.Value)).FirstOrDefault()?.Value;
                    context.Result = new ObjectResult(new MessageModel(Core.Enum.EnumResponseCode.ParameterError, msg));
                    _logger.LogWarning($"请求地址：{context.HttpContext.Request.Path}，参数验证错误：{msg}");
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IGNORE_CONTROLLERS.Contains(context.Controller.ToString()))
            {
                var httpContext = context.HttpContext;
                var url = $"HttpVerb:{httpContext.Request.Method},Path:{httpContext.Request.Path}";
                string? param;
                if (httpContext.Request.Method == "GET")
                {
                    param = httpContext.Request.QueryString.Value;
                }
                else
                {
                    Stream stream = httpContext.Request.Body;
                    byte[] buffer = new byte[httpContext.Request.ContentLength.GetValueOrDefault()];
                    stream.Read(buffer, 0, buffer.Length);
                    param = Encoding.UTF8.GetString(buffer);
                }
                await _logService.WriteLogAsync(url, param ?? "", context.ActionDescriptor.DisplayName, httpContext.Request.ContentType, true);
            }
            await next();
        }
    }
}
