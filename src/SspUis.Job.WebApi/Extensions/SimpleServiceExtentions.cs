using SspUis.Job.WebApi;
using WEBASE.i18n;
using WEBASE.Storage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleServiceExtentions
    {
        public static void ConfigureSimpleServices(this IServiceCollection services)
        {
            services.AddScoped<UploadFileAttribute>();
            services.AddScoped<ICultureHelper, CultureHelper>();
            services.AddSingleton<IDeleteOutdatedTempFilesJob, DeleteOutdatedTempFilesJob>();
            services.ConfigureEImzoServices(AppSettings.Instance.EImzo1_6);
        }
    }
}
