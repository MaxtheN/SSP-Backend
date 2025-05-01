using Hangfire;
using Hangfire.MemoryStorage;

namespace Microsoft.Extensions.DependencyInjection;

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
