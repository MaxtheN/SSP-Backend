using WbImzo.Proxy.Sdk;

namespace SspUis.My.WebApi.Extensions;

public static class WbImzoExtensions
{
    public static void ConfigureWebImzoServices1(this IServiceCollection services, WbImzoConfig config)
    {
        services.AddSingleton(AppSettings.Instance.Integration.WbImzoConfig);
        services.AddSingleton(AppSettings.Instance.LinkConfig);
        services.AddScoped<WbImzoHttpClient>();
        services.AddScoped<IWbImzoService, WbImzoService>();
    }
}
