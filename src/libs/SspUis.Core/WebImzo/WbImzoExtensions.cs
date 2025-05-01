using Microsoft.Extensions.DependencyInjection;
using SspUis.Core.Configurations;
using System.Collections.Generic;
using WbImzo.Proxy.Sdk;

namespace SspUis.Core.WebImzo
{
    public static class WbImzoExtensions
    {
        public static void ConfigureWebImzoServices(this IServiceCollection services, WbImzoConfig wbConfig, List<LinkConfig> linkConfigs)
        {
            services.AddSingleton(wbConfig);
            services.AddSingleton(linkConfigs);
            services.AddScoped<WbImzoHttpClient>();
            services.AddScoped<IWbImzoService, WbImzoService>();
        }
    }
}