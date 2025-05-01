using SspUis.Integration.Billing.Extensions;
using SspUis.Integration.Edoc.Extensions;
using SspUis.Integration.Investitsiya.Extensions;
using SspUis.Integration.Stat.Extension;
using SspUis.Integration.Sud.Extensions;
using WbAccessControl.Sdk;
using SspUis.Integration.AgroBank.Extensions;
using SspUis.Integration.XalqBank.Extensions;
using SspUis.Integration.OneId;
using SspUis.Core.Configurations;
using WbImzo.Proxy.Sdk;
using SspUis.Core.Extensions;
using SspUis.Core.WebImzo;
using WEBASE.Integration.Manuals.Extensions;

namespace SspUis.My.WebApi.Extensions;

public static class IntegrationExtensions
{
	public static IServiceCollection ConfigureIntegrations(this IServiceCollection services, IConfiguration conf)
	{
		services.AddIntegrationManuals();
		services.ConfigureOnlineMahallaServices(AppSettings.Instance.Integration.OnlineMahalla);
		services.ConfigureIntegrationCertificateServices(AppSettings.Instance.Integration.CertificatePostSoliq);
		services.ConfigureSoliqNetdocServices(AppSettings.Instance.Integration.SoliqNetdoc);
		services.ConfigureDavActivServices(AppSettings.Instance.Integration.DavAktiv);
		services.ConfigureBojxonaServices(AppSettings.Instance.Integration.Bojxona);
		services.ConfigureFinanceServices(AppSettings.Instance.Integration.Finance);
		services.ConfigureTadbirkorFundServices(AppSettings.Instance.Integration.TadbirkorFund);
		services.ConfigureMarkaziyBankServices(AppSettings.Instance.Integration.MarkaziyBank);
		services.ConfigureBandlikServices(AppSettings.Instance.Integration.Bandlik);
		services.ConfigureInvestitsiyaServices(AppSettings.Instance.Integration.Investitsiya);
		services.ConfigureIntegrationDigitizatonCenterServices(AppSettings.Instance.Integration.DigitizationCenter);
		services.ConfigureEdocRegistrationServices(AppSettings.Instance.Integration.EdocRegistration);
		services.ConfigureMspdServices(AppSettings.Instance.Integration.Mspd);
		services.ConfigureWbacServices(AppSettings.Instance.Integration.Wbac);
		services.AddSingleton(AppSettings.Instance.Integration.BasicAuth);
		services.ConfigureBillingServices(AppSettings.Instance.Integration.Billing);
		services.ConfigureDualServices(AppSettings.Instance.Integration.Dual);
		services.ConfigureStatServices(AppSettings.Instance.Integration.Stat);
		services.ConfigureSudServices(AppSettings.Instance.Integration.Sud);
		services.ConfigureIntegrationBankCreditServices(AppSettings.Instance.Integration.BankCredit);
		services.ConfigureDocxToPdfServices(AppSettings.Instance.Integration.DocxToPdfConfig);
		services.ConfigureOneIdServices(AppSettings.Instance.Integration.OneId);
		services.ConfigureIntegrationAgroBankServices(AppSettings.Instance.Integration.AgroBank);
		services.ConfigureIntegrationXalqBankServices(AppSettings.Instance.Integration.XalqBank);
		var linkConfigs = conf.GetSection("LinkConfigs").Get<List<LinkConfig>>();
		services.ConfigureWebImzoServices(AppSettings.Instance.WbImzoConfig, linkConfigs);
		services.ConfigureIntegrationAuth(AppSettings.Instance.Integration.Auth);
		return services;
	}
}
