using SspUis.Job.WebApi.Hangfire.JobServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireJobServiceExtentions
    {
        public static void ConfigureHangfireJobServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomJobServiceRunner, CustomJobServiceRunner>();
            services.AddScoped<ICustomJobHangfireService , CustomJobHangfireService>();
        }
    }
}
