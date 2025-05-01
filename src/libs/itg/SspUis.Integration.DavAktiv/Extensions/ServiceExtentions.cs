using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.DavAktiv;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureDavActivServices(this IServiceCollection services, DavAktivConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IDavAktivContractorService, DavAktivContractorService>();
            services.AddScoped<IDavAktivApplicationService, DavAktivApplicationService>();
        }
    }
}
