
using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.XalqBank.Configs;
using SspUis.Integration.XalqBank.Services;

namespace SspUis.Integration.XalqBank.Extensions;

public static class ServiceExtentions
{
     public static void ConfigureIntegrationXalqBankServices(this IServiceCollection services, XalqBankConfig config)
    {

        services.AddSingleton(config);
        services.AddScoped<IXalqBankService, XalqBankService>();
       
    }
}
