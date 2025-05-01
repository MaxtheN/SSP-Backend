using SspUis.Integration.BankCredit;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtentions
    {
        public static void ConfigureIntegrationBankCreditServices(this IServiceCollection services,
                                                          BankCreditConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IBankCreditService, BankCreditService>();
        }
    }
}
