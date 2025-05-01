using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WEBASE.Storage;
using SspUis.Job.WebApi;
using HangfireBasicAuthenticationFilter;

namespace Microsoft.AspNetCore.Builder
{
    public static class HangfireAppBuilderExtentions
    {
        public static void ConfigureHangfire(this IApplicationBuilder app)
        {
            if (AppSettings.Instance.HangFire.Enabled)
            {
                GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.ApplicationServices));

                app.UseHangfireDashboard("/JobManager", new DashboardOptions
                {
                    //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
                    DashboardTitle = "App Jobs",
                    Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter{
                                User = AppSettings.Instance.HangFire.Security.UserName,
                                Pass = AppSettings.Instance.HangFire.Security.Password
                            }
                    }
                });

                var api = JobStorage.Current.GetMonitoringApi();
                var processingJobs = api.ProcessingJobs(0, (int)api.ProcessingCount());
                var servers = api.Servers();
                var orphanJobs = processingJobs.Where(j => !servers.Any(s => s.Name == j.Value.ServerId));
                foreach (var orphanJob in orphanJobs)
                    BackgroundJob.Requeue(orphanJob.Key);

            }
        }
    }
}
