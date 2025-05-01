using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Notify.Sms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureNotifySmsServices(this IServiceCollection services, SmsProviderConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IProviderSmsService, ProviderSmsService>();
        }
    }
}
