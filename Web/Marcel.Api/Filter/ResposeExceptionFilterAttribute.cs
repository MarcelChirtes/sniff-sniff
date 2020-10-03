using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;

namespace Marcel.Api.Filter
{
    public class ResposeExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ResposeExceptionFilterAttribute> _logger;

        public ResposeExceptionFilterAttribute(ILogger<ResposeExceptionFilterAttribute> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError("ResposeExceptionFilter {Exception}", context.Exception);
            ComposeResponse(context);
            base.OnException(context);
        }

        private void ComposeResponse(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = DetermineHttpStatusCode(context.Exception);
            context.HttpContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            context.Result = DetermineBody(context.Exception);
        }

        private int DetermineHttpStatusCode(Exception exception)
        {
            switch (exception)
            {
                case UnauthorizedAccessException _:
                    return 401;

                default:
                    return 400;
            }
        }

        private JsonResult DetermineBody(Exception exception)
        {
            switch (exception)
            {
                case UnauthorizedAccessException _:
                    return new JsonResult(new
                    {
                        Message = exception.Message
                    });

                default:
                    return new JsonResult(new
                    {
                        ExceptionMessage = exception.Message,
                        exception.InnerException
                    });
            }
        }
    }
}