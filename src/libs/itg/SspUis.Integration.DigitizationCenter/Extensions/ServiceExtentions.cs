using SspUis.Integration.DigitizationCenter;
using SspUis.Integration.DigitizationCenter.Services.GSP;
using SspUis.Integration.DigitizationCenter.Soliq;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtentions
{
    public static void ConfigureIntegrationDigitizatonCenterServices(this IServiceCollection services, DigitizationCenterConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<IDigitizationCenterLoginService, DigitizationCenterLoginService>();
        services.AddScoped<IDigitizationCenterMehnatService, DigitizationCenterMehnatService>();
        services.AddScoped<IDigitizationCenterSoliqService, DigitizationCenterSoliqService>();
        services.AddScoped<IDigitizationCenterGspService, DigitizationCenterGspService>();
        services.AddScoped<IDigitizationCenterFHDYOService, DigitizationCenterFHDYOService>();
    }
}
