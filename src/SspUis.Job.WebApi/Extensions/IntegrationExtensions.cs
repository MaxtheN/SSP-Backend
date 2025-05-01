using SspUis.BizLogicLayer.IntegrationServices.Wbac;
using SspUis.Core.Configurations;
using SspUis.Core.WebImzo;
using SspUis.Integration.AgroBank.Extensions;
using SspUis.Integration.Billing.Extensions;
using SspUis.Integration.Edoc.Extensions;
using SspUis.Integration.Investitsiya.Extensions;
using SspUis.Integration.OneId;
using SspUis.Integration.Stat.Extension;
using SspUis.Integration.Sud.Extensions;
using SspUis.Integration.XalqBank.Extensions;
using WbAccessControl.Sdk;
using WEBASE.Integration.Manuals.Extensions;
using WEBASE.Notify.Sms;

namespace SspUis.Job.WebApi.Extensions;

public static class IntegrationExtensions
{
	public static IServiceCollection ConfigureIntegrations(this IServiceCollection services, IConfiguration conf)
	{
		services.AddIntegrationManuals();
		services.AddSingleton(AppSettings.Instance.Integration.BasicAuth);
		services.AddSingleton(AppSettings.Instance.Integration.SoliqOrgSettings);
		services.ConfigureMspdServices(AppSettings.Instance.Integration.Mspd);
		services.ConfigureEdocRegistrationServices(AppSettings.Instance.Integration.EdocRegistration);
		services.ConfigureOneIdServices(AppSettings.Instance.Integration.OneId);
		services.ConfigureWbacServices(AppSettings.Instance.Integration.Wbac);
		services.ConfigureOnlineMahallaServices(AppSettings.Instance.Integration.OnlineMahalla);
		services.ConfigureIntegrationCertificateServices(AppSettings.Instance.Integration.CertificatePost);
		services.ConfigureIntegrationBankCreditServices(AppSettings.Instance.Integration.BankCredit);
		services.ConfigureBojxonaServices(AppSettings.Instance.Integration.Bojxona);
		services.ConfigureSoliqNetdocServices(AppSettings.Instance.Integration.SoliqNetdoc);
		services.ConfigureDavActivServices(AppSettings.Instance.Integration.DavAktiv);
		services.ConfigureTadbirkorFundServices(AppSettings.Instance.Integration.TadbirkorFund);
		services.ConfigureIntegrationDigitizatonCenterServices(AppSettings.Instance.Integration.DigitizationCenter);
		services.ConfigureMarkaziyBankServices(AppSettings.Instance.Integration.MarkaziyBank);
		services.ConfigureNotifySmsServices(AppSettings.Instance.SmsProvider);
		services.ConfigureDocxToPdfServices(AppSettings.Instance.Integration.DocxToPdfConfig);
		services.ConfigureBandlikServices(AppSettings.Instance.Integration.Bandlik);
		services.ConfigureInvestitsiyaServices(AppSettings.Instance.Integration.Investitsiya);
		services.ConfigureFinanceServices(AppSettings.Instance.Integration.Finance);
		services.ConfigureIntegrationAgroBankServices(AppSettings.Instance.Integration.AgroBank);
		services.ConfigureBillingServices(AppSettings.Instance.Integration.Billing);
		services.ConfigureDualServices(AppSettings.Instance.Integration.Dual);
		services.ConfigureSudServices(AppSettings.Instance.Integration.Sud);
		services.ConfigureIntegrationXalqBankServices(AppSettings.Instance.Integration.XalqBank);
		services.ConfigureWbacIntegrationServices<WbacIntegrationService>();
		services.ConfigureStatServices(AppSettings.Instance.Integration.Stat);
		services.AddHttpClient<IProviderSmsService, ProviderSmsService>();

		//specify
		services.Configure<List<LinkConfig>>(conf.GetSection("LinkConfigs"));
		var linkConfigs = conf.GetSection("LinkConfigs").Get<List<LinkConfig>>();
		services.ConfigureWebImzoServices(AppSettings.Instance.WbImzoConfig, linkConfigs);

		return services;
	}
}
