using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.Bandlik;
using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Bandlik.Configs;
using SspUis.Integration.Bandlik.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureBandlikServices(this IServiceCollection services, BandlikConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IBandlikService, BandlikService>();
        }
    }
}
