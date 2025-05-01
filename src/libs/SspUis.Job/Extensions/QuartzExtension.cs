using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;
using WEBASE.QuartzForHrm.Configs;
using WEBASE.QuartzForHrm.Services;

namespace WEBASE.Quartz.Extensions;

public static class QuartzExtension
{
    public static void ConfigureQuartzServices(this IServiceCollection services, QuartzForHrmConfig quartzConfig)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(quartzConfig.JobKey);
            q.AddJob<AppointEmployeeJob>(opts => opts.WithIdentity(jobKey));
            q.AddJob<EmployeeLeaveOrderJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(quartzConfig.JobTrigger)
                 .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInHours(quartzConfig.Hours)
            .RepeatForever()));

        });

        //ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            //when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
    }
}