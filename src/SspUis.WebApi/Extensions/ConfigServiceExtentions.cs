using SspUis.WebApi;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigServiceExtentions
{
    public static void ConfigureConfigs(this IServiceCollection services)
    {
        services.AddSingleton(AppSettings.Instance.Jwt);
        services.AddSingleton(AppSettings.Instance.Culture);
        services.AddSingleton(AppSettings.Instance.System);
        services.AddSingleton(AppSettings.Instance.FileValidation);
        services.AddSingleton(AppSettings.Instance.Cookie);
        services.AddSingleton(AppSettings.Instance.Integration.BasicAuth);
    }
}