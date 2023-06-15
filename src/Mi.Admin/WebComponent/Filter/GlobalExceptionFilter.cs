using Mi.Core.Enum;
using Mi.Core.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Admin.WebComponent.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly MessageModel _message;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(MessageModel message, ILogger<GlobalExceptionFilter> logger)
        {
            _message = message;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
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
            }
            context.ExceptionHandled = true;
        }
    }
}