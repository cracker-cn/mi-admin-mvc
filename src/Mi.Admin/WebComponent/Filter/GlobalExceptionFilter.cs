using Mi.Core.Enum;
using Mi.Core.Models;
using Mi.Core.Toolkit.Helper;
using Mi.IService.System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Admin.WebComponent.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly MessageModel _message;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly ILogService _logService;

        public GlobalExceptionFilter(MessageModel message, ILogger<GlobalExceptionFilter> logger, ILogService logService)
        {
            _message = message;
            _logger = logger;
            _logService = logService;
        }

        public void OnException(ExceptionContext context)
        {
            var items = context.HttpContext.Items;
            if (!context.ExceptionHandled)
            {
                if (context.Exception is Ouch)
                {
                    context.Result = new ObjectResult(_message.Fail(context.Exception.Message));
                }
                else
                {
                    context.Result = new ObjectResult(new MessageModel(EnumResponseCode.Error, context.Exception.Message));
                }
                _logger.LogError(context.Exception, context.Exception.Message);
                if (context.HttpContext.Items.TryGetValue("RequestId", out var temp))
                {
                    var guid = (string?)temp;
                    (_logService.SetExceptionAsync(guid ?? IdHelper.UUID(), context.Exception.Message)).ConfigureAwait(true);
                }
            }
            context.ExceptionHandled = true;
        }
    }
}