using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.Bojxona;
using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Bojxona.Configs;
using SspUis.Integration.Bojxona.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureBojxonaServices(this IServiceCollection services, BojxonaConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IBojxonaService, BojxonaService>();
        }
    }
}
