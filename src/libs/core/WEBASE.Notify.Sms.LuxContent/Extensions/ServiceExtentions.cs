using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Notify.Sms;
using WEBASE.Notify.Sms.LuxContent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureLuxContentSmsServices(this IServiceCollection services, LuxContentSmsProviderConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<ILuxContentSmsService, LuxContentSmsService>();
        }
    }
}
