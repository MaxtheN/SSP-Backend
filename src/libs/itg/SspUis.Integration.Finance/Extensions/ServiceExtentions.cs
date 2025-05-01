using SspUis.Integration.Finance.Configs;
using SspUis.Integration.Finance.Services;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureFinanceServices(this IServiceCollection services, FinanceConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IFinanceService, FinanceService>();
        }
    }
}
