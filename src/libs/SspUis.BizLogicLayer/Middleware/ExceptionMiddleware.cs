using DocumentFormat.OpenXml.InkML;
using SspUis.BizLogicLayer.AppErrorServices;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SspUis.BizLogicLayer
{
    public class ExceptionMiddlewareOptions
    {
        public bool WriteToDb { get; set; }
        public bool IncludeBody { get; set; }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ExceptionMiddlewareOptions _options;

        public ExceptionMiddleware(
            RequestDelegate next,
            IOptions<ExceptionMiddlewareOptions> options,
            ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _options = options.Value;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAppErrorService appErrorService, IAuthService authService)
        {
            try
            {
                if (_options.IncludeBody)
                    httpContext.Request.EnableBuffering();

                await _next(httpContext);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogWarning(ex, "BadHttpRequestException");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                await HandleExceptionAsync(httpContext, ex, appErrorService, authService);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IAppErrorService appErrorService, IAuthService authService)
        {
            try
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var detail = $"StackTrace: {Environment.NewLine}{exception.StackTrace}{Environment.NewLine} Full exception: {Environment.NewLine}{exceptionHandlerFeature?.Error}";
                var error = new CreateAppErrorDlDto
                {
                    UserId = authService != null && authService.IsAuthenticated ? (int)authService.UserId : 0,
                    UserName = authService != null && authService.IsAuthenticated ? authService.UserName : "",
                    Host = context.Request.Host.Value,
                    RequestPath = context.Request.Path,
                    RequestBody = await ReadRequestBodyAsync(context),
                    StatusCode = 500,
                    Type = exception.GetType().Name,
                    Title = exception.Message,
                    RequestTraceId = context.TraceIdentifier,
                    UserAgent = authService.UserAgent,
                    IpAddress = authService.UserIp,
                    Detail = detail.Length > 5000 ? detail.Substring(0, 5000) : detail
                };

                if (_options.WriteToDb)
                {
                    // write to db
                    appErrorService.Create(error);
                }

                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    error.RequestTraceId,
                    error.StatusCode,
                    error.Type,
                    error.Title,
                    error.Detail
                }));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while handling exception in CUSTOM_MIDDLEWARE.");
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            if (!_options.IncludeBody)
                return null;

            using (var sr = new StreamReader(context.Request.Body))
            {
                context.Request.Body.Position = 0;
                var requestBody = await sr.ReadToEndAsync();

                return requestBody;
            }
        }
    }
}
