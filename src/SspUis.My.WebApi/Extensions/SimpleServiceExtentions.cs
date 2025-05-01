using SspUis.My.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.i18n;
using WEBASE.Storage;
using WEBASE.Minio;

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
