using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace MyWallet.ErrorHandlers
{
    public class WebAPIExceptionFilter: IExceptionFilter
    {
        private Serilog.ILogger _logger;
        public WebAPIExceptionFilter(Serilog.ILogger logger)
        {
            _logger = logger.ForContext<WebAPIExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            WebAPIError apiError = null;
            if(context.Exception is WebAPIException)
            {
                var ex = context.Exception as WebAPIException;
                context.Exception = null;
                apiError = new WebAPIError(ex.Message);

                context.HttpContext.Response.StatusCode = (int)ex.StatusCode;
                _logger.Warning($"MyWallet API thrown error: {ex.Message}", ex);

            } else if(context.Exception is UnauthorizedAccessException)
            {
                apiError = new WebAPIError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
            } else
            {
#if !DEBUG
                  var msg = "An unhandled error ocurred.";
                  string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif
                apiError = new WebAPIError(msg);
                apiError.Detail = stack;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // handle logging here
                _logger.Error(context.Exception, msg);
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);
            context.ExceptionHandled = true;
        }
    }
}
