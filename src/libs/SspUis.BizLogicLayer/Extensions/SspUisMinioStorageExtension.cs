using SspUis.BizLogicLayer.Minio;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Minio;
using WEBASE.Storage;

namespace WEBASE.Integration.Manuals.Extensions
{
    public static class SspUisMinioStorageExtension
    {
        public static IServiceCollection AddSspUisMinio(this IServiceCollection services, MinioConfig config)
        {
            services.AddSingleton((FileStorageConfig)config);
            services.AddSingleton(config);
            services.AddScoped<IStorageService, SspUisMinioStorageService>();
            return services;
        }
    }
}
