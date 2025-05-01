using SspUis.Integration.Certificate;
using SspUis.Integration.IntegrationCertificate;
using SspUis.Integration.IntegrationCertificateMB;
using SspUis.Integration.IntegrationCertificateXalqBank;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtentions
    {
        public static void ConfigureIntegrationCertificateServices(this IServiceCollection services,
                                                          IntegrationCertificateConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IIntegrationCertificateService, IntegrationCertificateService>();
            services.AddScoped<IIntegrationCertificateMBService, IntegrationCertificateMBService>();
            services.AddScoped<IIntegrationCertificateBojxonaService, IntegrationCertificateBojxonaService>();
            services.AddScoped<IIntegrationCertificateXalqBankService, IntegrationCertificateXalqBankService>();
            services.AddScoped<IIntegrationCertificateMoliyaService, IntegrationCertificateMoliyaService>();
        }
    }
}
