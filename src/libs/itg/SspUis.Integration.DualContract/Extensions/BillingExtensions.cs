

using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Dual.Configs;
using SspUis.Integration.Dual.Services;


namespace SspUis.Integration.Billing.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDualServices(this IServiceCollection services, DualConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<IDualService, DualService>();
    }
}
