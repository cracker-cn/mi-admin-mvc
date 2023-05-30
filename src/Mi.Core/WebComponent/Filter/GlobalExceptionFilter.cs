using Mi.Core.Extension;
using Mi.Core.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Core.WebComponent.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly MessageModel _message;

        public GlobalExceptionFilter(MessageModel message)
        {
            _message = message;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                //context.ActionDescriptor
                context.Result = new ObjectResult(_message.Fail(context.Exception.Message));
            }
            context.ExceptionHandled = true;
            var t = context.Result.GetType();
            Console.WriteLine("=======hello");
        }
    }
}