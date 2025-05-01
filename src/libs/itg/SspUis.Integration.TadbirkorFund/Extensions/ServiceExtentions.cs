using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.TadbirkorFund;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureTadbirkorFundServices(this IServiceCollection services, TadbirkorFundConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<ITadbirkorFundContractorService, TadbirkorFundContractorService>();
        }
    }
}
