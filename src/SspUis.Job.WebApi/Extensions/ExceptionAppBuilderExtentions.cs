using SspUis.Job.WebApi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionAppBuilderExtentions
    {
        public static void ConfigureExceptions(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null && AppSettings.Instance.ClientErrors.Enabled)
                    {
                        var ex = error.Error.GetInnermostException();
                        var problem = new ProblemDetails
                        {
                            Status = 500,
                            Type = error.Error.GetType().Name,
                            Title = ex.Message
                        };

                        if (AppSettings.Instance.ClientErrors.SentErrorDetails)
                            problem.Detail = $"StackTrace: {Environment.NewLine}{ex.StackTrace}{Environment.NewLine} Full exception: {Environment.NewLine}{error.Error}";

                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problem));
                    }
                });
            });
        }
    }
}
