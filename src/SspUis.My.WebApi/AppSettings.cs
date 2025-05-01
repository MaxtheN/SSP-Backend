using SspUis.Core.Configurations;
using SspUis.Integration.AgroBank.Configs;
using SspUis.Integration.Bandlik.Configs;
using SspUis.Integration.BankCredit;
using SspUis.Integration.Billing.Configs;
using SspUis.Integration.Bojxona.Configs;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.DigitizationCenter;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Dual.Configs;
using SspUis.Integration.Edoc.Configs;
using SspUis.Integration.Finance.Configs;
using SspUis.Integration.IntegrationCertificate;
using SspUis.Integration.Investitsiya.Configs;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.OneId;
using SspUis.Integration.OnlineMahalla;
using SspUis.Integration.Soliq;
using SspUis.Integration.Stat.Configs;
using SspUis.Integration.Sud.Configs;
using SspUis.Integration.TadbirkorFund;
using SspUis.Integration.XalqBank.Configs;
using WbAccessControl.Sdk;
using WbImzo.Proxy.Sdk;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Integration.MSPD.Client;
using WEBASE.Minio;
using WEBASE.Notify.Sms;
using WEBASE.Notify.Sms.LuxContent;

namespace SspUis.My.WebApi
{
	public class AppSettings
	{
		public static AppSettings Instance { get; private set; }
		public WbImzoConfig WbImzoConfig { get; set; } = new WbImzoConfig();
		public LinkConfig LinkConfig { get; set; } = new LinkConfig();
		public CultureConfig Culture { get; set; } = new();
		public ClientErrorsConfig ClientErrors { get; set; } = new();
		public DatabaseConfig Database { get; set; } = new();
		public CookieConfig Cookie { get; set; } = new();
		public SwaggerConfig Swagger { get; set; } = new();
		public ControllerConfig ControllerConfig { get; set; } = new();
		public IntegrationConfig Integration { get; set; } = new();
		public SystemConf System { get; set; } = new();
		public SmsProviderConfig SmsProvider { get; set; } = new();
		public LuxContentSmsProviderConfig LuxContentSmsProvider { get; set; } = new();
		public EImzoConfig EImzo1_6 { get; set; } = new();
		public CorsConfig Cors { get; set; } = new();
		public MinioConfig Minio { get; set; } = new();

		public static void Init(AppSettings instance)
		{
			Instance = instance;
		}
	}

	public class ClientErrorsConfig
	{
		public bool Enabled { get; set; }
		public bool SentErrorDetails { get; set; }
	}

	public class DatabaseConfig
	{
		public PgSqlConfig PgSql { get; set; } = new();
	}

	public class PgSqlConfig
	{
		public string ConnectionString { get; set; } = "";
	}
	public class SwaggerConfig
	{
		public bool Enabled { get; set; }
		public string Prefix { get; set; }
	}

	public class CorsConfig
	{
		public bool UseCors { get; set; }
		public string[] AllowedOrigins { get; set; }
	}

	public class IntegrationConfig
	{
		public WbImzoConfig WbImzoConfig { get; set; } = new WbImzoConfig();
		public BasicAuthConfig BasicAuth { get; set; } = new();
		public IntegrationAuthConfig Auth { get; set; } = new();
		public SoliqConfig SoliqNetdoc { get; set; } = new();
		public DavAktivConfig DavAktiv { get; set; } = new();
		public TadbirkorFundConfig TadbirkorFund { get; set; } = new TadbirkorFundConfig();
		public MarkaziyBankConfig MarkaziyBank { get; set; } = new MarkaziyBankConfig();
		public MspdConfig Mspd { get; set; } = new();
		public BojxonaConfig Bojxona { get; set; } = new();
		public OnlineMahallaConfig OnlineMahalla { get; set; } = new();
		public IntegrationCertificateConfig CertificatePostSoliq { get; set; } = new();
		public DocxToPdfConfig DocxToPdfConfig { get; set; } = new();
		public BandlikConfig Bandlik { get; set; } = new();
		public BankCreditConfig BankCredit { get; set; } = new();
		public BillingConfig Billing { get; set; } = new();
		public InvestitsiyaConfig Investitsiya { get; set; } = new();
		public WbacConfig Wbac { get; set; } = new();
		public DigitizationCenterConfig DigitizationCenter { get; set; } = new();
		public EdocRegistrationConfig EdocRegistration { get; set; } = new();
		public OneIdConfig OneId { get; set; } = new();
		public AgroBankConfig AgroBank { get; set; } = new();
		public FinanceConfig Finance { get; set; } = new();
		public StatConfig Stat { get; set; } = new();
		public DualConfig Dual { get; set; } = new();
		public SudConfig Sud { get; set; } = new();
		public XalqBankConfig XalqBank { get; set; } = new();
	}
}