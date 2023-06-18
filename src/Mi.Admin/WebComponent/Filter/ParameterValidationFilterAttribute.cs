using Mi.Core.CommonOption;
using Mi.Core.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Admin.WebComponent.Filter
{
    public class ParameterValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ParameterValidationFilterAttribute> _logger;

        public ParameterValidationFilterAttribute(ILogger<ParameterValidationFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys.Where(key => context.ModelState[key] != null).Select(key => new Option { Name = key, Value = context.ModelState[key]!.Errors.FirstOrDefault()?.ErrorMessage });
                if (result != null)
                {
                    var msg = result.FirstOrDefault()?.Value;
                    context.Result = new ObjectResult(new MessageModel(Core.Enum.EnumResponseCode.ParameterError, msg));
                    _logger.LogWarning($"请求地址：{context.HttpContext.Request.Path}，参数验证错误：{msg}");
                }
            }
        }
    }
}
