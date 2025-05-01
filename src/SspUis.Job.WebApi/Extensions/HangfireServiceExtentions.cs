using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using SspUis.Job.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireServiceExtentions
    {
        public static void ConfigureHangfireServices(this IServiceCollection services)
        {
            if (AppSettings.Instance.HangFire.Enabled)
            {
                services.ConfigureHangfireJobServices();

                if (string.IsNullOrEmpty(AppSettings.Instance.HangFire.Storage?.PostgreSql?.ConnectionString))
                    services.AddHangfire(config =>
                    {
                        config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseRecommendedSerializerSettings()
                        .UseDefaultTypeSerializer()
                        .UseMemoryStorage();
                    });
                else
                    services.AddHangfire(config =>
                    {
                        config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseRecommendedSerializerSettings()
                        .UseDefaultTypeSerializer()
                        .UsePostgreSqlStorage(AppSettings.Instance.HangFire.Storage.PostgreSql.ConnectionString);
                    });

                if (AppSettings.Instance.HangFire.JobServers != null)
                {
                    AppSettings.Instance.HangFire.JobServers.ForEach(jobServer =>
                    {
                        if (jobServer.Queues != null && jobServer.Queues.Count > 0)
                        {
                            // Add the processing server as IHostedService
                            services.AddHangfireServer(s =>
                            {
                                s.WorkerCount = jobServer.WorkerCount; s.ServerName = jobServer.ServerName;
                                s.Queues = jobServer.Queues.Select(x => x.QueueName).ToArray();
                            });// 
                        }
                    });
                }
            }
        }
    }
}
