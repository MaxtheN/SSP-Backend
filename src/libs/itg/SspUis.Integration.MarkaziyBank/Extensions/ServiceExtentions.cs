using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.Integration.MarkaziyBank;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureMarkaziyBankServices(this IServiceCollection services, MarkaziyBankConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IMarkaziyBankContractorService, MarkaziyBankContractorService>();
        }
    }
}
