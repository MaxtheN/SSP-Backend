using Quartz;
using Quartz.AspNetCore;
using SspUis.BizLogicLayer.IntegrationServices;
using SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
using SspUis.BizLogicLayer.IntegrationServices.Xodim;
namespace SspUis.Job.WebApi.Extensions
{
	public static class QuartzServiceExtensions
	{
		public static void ConfigureQuartzServices(this IServiceCollection services)
		{
			services.AddQuartz(q =>
			{
				var jobKey = new JobKey(AppSettings.Instance.Quartz.JobKey);
				var financejobKey = new JobKey(AppSettings.Instance.FinanceQuartz.JobKey);
				var mehnatJobKey = new JobKey(AppSettings.Instance.MehnatQuartz.JobKey);
				var workdActivityJobKey = new JobKey(AppSettings.Instance.WorkActivityQuartz.JobKey);
				var xodimPhotoJobKey = new JobKey(AppSettings.Instance.XodimPhotoQuartz.JobKey);

				services.AddScoped<IFinanceIntegrationService, FinanceIntegrationService>();
				services.AddScoped<IMehnatService, MehnatService>();
				services.AddScoped<IWorkActivity, WorkActivity>();
				services.AddScoped<IXodimPhotoUploader, XodimPhotoUploader>();


				q.AddJob<CreateIntegrationApiLog>(opts => opts.WithIdentity(jobKey));
				q.AddTrigger(opts => opts
				.ForJob(jobKey)
				.WithIdentity(AppSettings.Instance.Quartz.JobTrigger)
				 .StartNow()
			.WithSimpleSchedule(x => x
			.WithIntervalInSeconds(AppSettings.Instance.Quartz.Hours)
			.RepeatForever()));

				q.AddJob<FinanceIntegrationJobService>(opts => opts.WithIdentity(financejobKey));
				q.AddTrigger(opts => opts
				.ForJob(financejobKey)
				.WithIdentity(AppSettings.Instance.FinanceQuartz.JobTrigger)
				 .StartNow()
			.WithSimpleSchedule(x => x
			.WithIntervalInHours(AppSettings.Instance.FinanceQuartz.Hours)

			.RepeatForever()));

				DateTimeOffset startTime = DateTimeOffset.Now;
				if (startTime.Hour >= 23 && startTime.Minute > 5)
				{
					startTime = startTime.AddDays(1);
				}
				startTime = new DateTimeOffset(startTime.Year, startTime.Month, startTime.Day, 23, 5, 0, startTime.Offset);

				q.AddJob<WorkActivityJobService>(opts => opts.WithIdentity(workdActivityJobKey)); // Register the new job
				q.AddTrigger(opts => opts
				 .ForJob(workdActivityJobKey)
				 .WithIdentity(AppSettings.Instance.WorkActivityQuartz.JobTrigger) // Add a trigger for the new job
				 .StartAt(startTime)
				 .WithSimpleSchedule(x => x
				 .WithIntervalInHours(AppSettings.Instance.WorkActivityQuartz.Hours) // Configure the interval
				 .RepeatForever()));

				DateTimeOffset startTime2 = DateTimeOffset.Now;
				if (startTime2.Hour >= 21 && startTime2.Minute > 5)
				{
					startTime2 = startTime2.AddDays(1);
				}
				startTime2 = new DateTimeOffset(startTime2.Year, startTime2.Month, startTime2.Day, 21, minute: 5, 0, startTime2.Offset);

				q.AddJob<MehnatJobService>(opts => opts.WithIdentity(mehnatJobKey)); // Register the new job
				q.AddTrigger(opts => opts
				 .ForJob(mehnatJobKey)
				 .WithIdentity(AppSettings.Instance.MehnatQuartz.JobTrigger) // Add a trigger for the new job
				 .StartAt(startTime2)
				 .WithSimpleSchedule(x => x
				 .WithIntervalInHours(AppSettings.Instance.MehnatQuartz.Hours) // Configure the interval
				 .RepeatForever()));


				DateTimeOffset startTime3 = DateTimeOffset.Now;
				if (startTime3.Hour >= 23 && startTime3.Minute > 40)
				{
					startTime3 = startTime3.AddDays(1);
				}
				startTime3 = new DateTimeOffset(startTime3.Year, startTime3.Month, startTime3.Day, 23, minute: 40, 0, startTime3.Offset);

				q.AddJob<XodimPhotoUploaderJobService>(opts => opts.WithIdentity(xodimPhotoJobKey)); // Register the new job
				q.AddTrigger(opts => opts
				 .ForJob(xodimPhotoJobKey)
				 .WithIdentity(AppSettings.Instance.XodimPhotoQuartz.JobTrigger) // Add a trigger for the new job
				 .StartAt(startTime3)
				 .WithSimpleSchedule(x => x
				 .WithIntervalInHours(AppSettings.Instance.XodimPhotoQuartz.Hours) // Configure the interval
				 .RepeatForever()));

				//This Cron interval can be described as "run every minute"(when second is zero)
				//.WithCronSchedule("1 * * ? * *"));
			});

			// ASP.NET Core hosting
			services.AddQuartzServer(options =>
			{
				// when shutting down we want jobs to complete gracefully
				options.WaitForJobsToComplete = true;
			});
		}
	}
}
