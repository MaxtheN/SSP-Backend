using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireServiceExtentions
    {
        public static void ConfigureHangfireServices(this IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseRecommendedSerializerSettings()
                      .UseDefaultTypeSerializer()
                      .UseMemoryStorage();
            });
        }
    }
}
