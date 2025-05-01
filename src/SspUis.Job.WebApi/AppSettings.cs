using SspUis.BizLogicLayer.FileValidationServices;
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
using SspUis.Job.WebApi.Configs;
using WbAccessControl.Sdk;
using WbImzo.Proxy.Sdk;
using WEBASE.AspNet.Security;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Integration.MSPD.Client;
using WEBASE.Minio;
using WEBASE.Notify.Sms;

namespace SspUis.Job.WebApi;

public class AppSettings
{
	public static AppSettings Instance { get; private set; }

	public WbImzoConfig WbImzoConfig { get; set; } = new WbImzoConfig();
	public LinkConfig LinkConfig { get; set; } = new LinkConfig();
	public string TestByUserName { get; set; }
	public FileValidationConfig FileValidation { get; set; } = new();
	public CultureConfig Culture { get; set; } = new CultureConfig();
	public CookieConfig Cookie { get; set; } = new();
	public ClientErrorsConfig ClientErrors { get; set; } = new ClientErrorsConfig();
	public DatabaseConfig Database { get; set; }
	public JwtConfig Jwt { get; set; }
	public SwaggerConfig Swagger { get; set; } = new SwaggerConfig();
	public ControllerConfig ControllerConfig { get; set; } = new ControllerConfig();
	public IntegrationConfig Integration { get; set; } = new IntegrationConfig();
	public SystemConf System { get; set; } = new SystemConf();
	public CorsConfig Cors { get; set; } = new();
	#region quartz jobs
	public QuartzConfig Quartz { get; set; } = new();
	public FinanceQuartzConfig FinanceQuartz { get; set; } = new();
	public MehnatQuartzConfig MehnatQuartz { get; set; } = new();
	public WorkActivityQuartzConfig WorkActivityQuartz { get; set; } = new();
	public XodimPhotoConfig XodimPhotoQuartz { get; set; } = new();
	#endregion
	public MinioConfig Minio { get; set; } = new();
	public EImzoConfig EImzo1_6 { get; set; } = new();
	public SmsProviderConfig SmsProvider { get; set; } = new();
	public HangFireConfig HangFire { get; set; } = new();
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
	public PgSqlConfig PgSql { get; set; }
}

public class PgSqlConfig
{
	public string ConnectionString { get; set; }
}

public class SwaggerConfig
{
	public bool Enabled { get; set; }
	public string Prefix { get; set; }
}

public class ControllerConfig
{
	public bool EnableSecurityInfo { get; set; }
}

public class CorsConfig
{
	public bool UseCors { get; set; }
	public string AllowedOrgins { get; set; }
}

public class QuartzConfig
{
	public string JobKey { get; set; }
	public string JobTrigger { get; set; }
	public int Hours { get; set; }
}
public class FinanceQuartzConfig
{
	public string JobKey { get; set; }
	public string JobTrigger { get; set; }
	public int Hours { get; set; }
}

public class MehnatQuartzConfig
{
	public string JobKey { get; set; }
	public string JobTrigger { get; set; }
	public int Hours { get; set; }
}

public class WorkActivityQuartzConfig
{
	public string JobKey { get; set; }
	public string JobTrigger { get; set; }
	public int Hours { get; set; }
}

public class XodimPhotoConfig
{
	public string JobKey { get; set; }
	public string JobTrigger { get; set; }
	public int Hours { get; set; }
}
public class IntegrationConfig
{
	public SoliqOrgConfig SoliqOrgSettings { get; set; }
	public BasicAuthConfig BasicAuth { get; set; } = new();
	public SoliqConfig SoliqNetdoc { get; set; } = new SoliqConfig();
	public DavAktivConfig DavAktiv { get; set; } = new DavAktivConfig();
	public TadbirkorFundConfig TadbirkorFund { get; set; } = new TadbirkorFundConfig();
	public MarkaziyBankConfig MarkaziyBank { get; set; } = new MarkaziyBankConfig();
	public MspdConfig Mspd { get; set; } = new();
	public DocxToPdfConfig DocxToPdfConfig { get; set; } = new();
	public OnlineMahallaConfig OnlineMahalla { get; set; } = new();
	public BojxonaConfig Bojxona { get; set; } = new();
	public BankCreditConfig BankCredit { get; set; } = new();
	public BandlikConfig Bandlik { get; set; } = new();
	public EdocRegistrationConfig EdocRegistration { get; set; } = new();
	public OneIdConfig OneId { get; set; }
	public IntegrationCertificateConfig CertificatePost { get; set; } = new();
	public InvestitsiyaConfig Investitsiya { get; set; } = new();
	public WbacConfig Wbac { get; set; } = new();
	public DigitizationCenterConfig DigitizationCenter { get; set; } = new();
	public FinanceConfig Finance { get; set; } = new();
	public AgroBankConfig AgroBank { get; set; } = new();
	public BillingConfig Billing { get; set; } = new();
	public DualConfig Dual { get; set; } = new();
	public SudConfig Sud { get; set; } = new();
	public StatConfig Stat { get; set; } = new();
	public XalqBankConfig XalqBank { get; set; } = new();
}
