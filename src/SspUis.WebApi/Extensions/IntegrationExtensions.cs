using SspUis.Integration.AgroBank.Extensions;
using SspUis.Integration.Billing.Extensions;
using SspUis.Integration.Edoc.Extensions;
using SspUis.Integration.Investitsiya.Extensions;
using SspUis.Integration.OneId;
using SspUis.Integration.Stat.Extension;
using SspUis.Integration.Sud.Extensions;
using SspUis.Integration.XalqBank.Extensions;
using SspUis.BizLogicLayer.IntegrationServices.Wbac;
using WbAccessControl.Sdk;
using SspUis.WebApi;
using SspUis.Core.Configurations;
using WEBASE.Notify.Sms;
using SspUis.Core.WebImzo;
using WEBASE.Integration.Manuals.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class IntegrationExtensions
{
    public static IServiceCollection ConfigureIntegrations(this IServiceCollection services, IConfiguration conf)
    {
        services.AddIntegrationManuals();
        services.ConfigureMspdServices(AppSettings.Instance.Integration.Mspd);
        services.ConfigureWbacServices(AppSettings.Instance.Integration.Wbac);
        services.ConfigureWbacIntegrationServices<WbacIntegrationService>();
        services.ConfigureEdocRegistrationServices(AppSettings.Instance.Integration.EdocRegistration);
        services.ConfigureOnlineMahallaServices(AppSettings.Instance.Integration.OnlineMahalla);
        services.ConfigureIntegrationCertificateServices(AppSettings.Instance.Integration.CertificatePost);
        services.ConfigureSoliqNetdocServices(AppSettings.Instance.Integration.SoliqNetdoc);
        services.ConfigureBojxonaServices(AppSettings.Instance.Integration.Bojxona);
        services.ConfigureDavActivServices(AppSettings.Instance.Integration.DavAktiv);
        services.ConfigureTadbirkorFundServices(AppSettings.Instance.Integration.TadbirkorFund);
        services.ConfigureMarkaziyBankServices(AppSettings.Instance.Integration.MarkaziyBank);
        services.ConfigureBandlikServices(AppSettings.Instance.Integration.Bandlik);
        services.ConfigureInvestitsiyaServices(AppSettings.Instance.Integration.Investitsiya);
        services.ConfigureIntegrationBankCreditServices(AppSettings.Instance.Integration.BankCredit);
        services.ConfigureIntegrationDigitizatonCenterServices(AppSettings.Instance.Integration.DigitizationCenter);
        services.ConfigureOneIdServices(AppSettings.Instance.Integration.OneId);
        services.ConfigureIntegrationAgroBankServices(AppSettings.Instance.Integration.AgroBank);
        services.ConfigureIntegrationXalqBankServices(AppSettings.Instance.Integration.XalqBank);
        services.ConfigureFinanceServices(AppSettings.Instance.Integration.Finance);
        services.ConfigureBillingServices(AppSettings.Instance.Integration.Billing);
        services.ConfigureDualServices(AppSettings.Instance.Integration.Dual);
        services.ConfigureSudServices(AppSettings.Instance.Integration.Sud);
        services.ConfigureStatServices(AppSettings.Instance.Integration.Stat);
        services.ConfigureDocxToPdfServices(AppSettings.Instance.Integration.DocxToPdfConfig);

        // specify
        services.AddSingleton(AppSettings.Instance.Integration.SoliqOrgSettings);
        services.ConfigureNotifySmsServices(AppSettings.Instance.SmsProvider);
        services.AddHttpClient<IProviderSmsService, ProviderSmsService>();
        services.Configure<List<LinkConfig>>(conf.GetSection("LinkConfigs"));
        var linkConfigs = conf.GetSection("LinkConfigs").Get<List<LinkConfig>>();
        services.ConfigureWebImzoServices(AppSettings.Instance.WbImzoConfig, linkConfigs);

        return services;
    }
}
