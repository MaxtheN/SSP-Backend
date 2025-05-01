using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WEBASE.Storage;

namespace Microsoft.AspNetCore.Builder
{
    public static class HangfireAppBuilderExtentions
    {
        public static void ConfigureHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireServer();
            var deleteOutdatedTempFilesJob = app.ApplicationServices.GetService<IDeleteOutdatedTempFilesJob>();
            RecurringJob.AddOrUpdate(nameof(IDeleteOutdatedTempFilesJob), () => deleteOutdatedTempFilesJob.Run(), Cron.Hourly());
        }
    }
}
