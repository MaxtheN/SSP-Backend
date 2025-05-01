using SspUis.Job.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorsServiceExtentions
    {
        public static void ConfigureCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                options.AddPolicy("AllowedOrgins",
                builder =>
                {
                    if (AppSettings.Instance.Cors.UseCors)
                        builder
                        .WithOrigins(AppSettings.Instance.Cors.AllowedOrgins.Split(new string[] { ",", ";" }, StringSplitOptions.None))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    else
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        /*.AllowCredentials()*/;
                });

            });
        }
    }
}
