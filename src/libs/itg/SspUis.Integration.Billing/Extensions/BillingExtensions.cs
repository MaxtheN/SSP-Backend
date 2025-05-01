using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Billing.Configs;
using SspUis.Integration.Billing.Services;

namespace SspUis.Integration.Billing.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureBillingServices(this IServiceCollection services, BillingConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<IBillingService, BillingService>();
        //services.AddScoped<DualApplicationTableDtoConfig>();
    }
}