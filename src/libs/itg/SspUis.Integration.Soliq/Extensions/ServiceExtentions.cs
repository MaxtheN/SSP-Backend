using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.Soliq;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureSoliqNetdocServices(this IServiceCollection services, SoliqConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<ISoliqContractorService, SoliqContractorService>();
        }
    }
}
