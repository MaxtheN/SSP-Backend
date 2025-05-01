using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Investitsiya.Configs;
using SspUis.Integration.Investitsiya.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Investitsiya.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureInvestitsiyaServices(this IServiceCollection services, InvestitsiyaConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IInvestitsiyaService, InvestitsiyaService>();
        }
    }
}