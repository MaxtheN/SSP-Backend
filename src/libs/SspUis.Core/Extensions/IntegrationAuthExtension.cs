using Microsoft.Extensions.DependencyInjection;
using SspUis.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core.Extensions
{
    public static class IntegrationAuthExtensions
    {
        public static void ConfigureIntegrationAuth(this IServiceCollection services, IntegrationAuthConfig config)
        {
            services.AddSingleton(config);
        }
    }
}
