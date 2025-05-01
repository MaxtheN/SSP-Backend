using SspUis.Core;
namespace SspUis.BizLogicLayer.Memship;
public class MemshipDashTotalStatisticsDto
{
    public int TotalApplicationsAcceptedCount { get; set; }
    public int TotalApplicationsReviewCount { get; set; }
    public int TotalApplicationsCanceledCount { get; set; }
    public int TotalContractReviewCount { get; set; }
    public int TotalContractSingingCount { get; set; }
    public int TotalContractSignedCount { get; set; }
    public int TotalContractCanceledCount { get; set; }
    public int TotalCertificateFormedCount { get; set; }
    public long Income { get; set; }
    public long Debt { get; set; }
}
public class MemshipDashDocsDto : MemshipDashFilterOption
{
    public long DocumentCount { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
}
public class MemshipContractTypeDto
{
    public int ContractCategoryTypeId { get; set; }
    public string ContractCategoryType { get; set; }
    public long ApplicationCount { get; set; }
    public long ContractCount { get; set; }
    public long CertificateCount { get; set; }
}
public class MemshipContractRate : MemshipDashFilterOption
{
    public string RegionOrderCode { get; set; }
    public string DistrictOrderCode { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
}
public static class ConstStatusParams
{
    public const string FORMED_CERTIFICATE_NAME = "Sertificate berilgan";

    public const string SIGNED_CONTRACT_NAME = "Shartnoma tuzilgan";

    public const string SIGNING_CONTRACT_NAME = "Ko'rib chiqilmoqda";

    public static int[] ApplicationStatuses { get; private set; } = new[]
    { 
        StatusIdConst.REJECTED, StatusIdConst.SENT, StatusIdConst.ACCEPTED 
    };

    public static int[] ContractStatuses { get; private set; } = new[]
    { 
        StatusIdConst.CREATED, StatusIdConst.REJECTED, StatusIdConst.SIGNED, StatusIdConst.SIGNING 
    };

    public static int[] CertificateStatuses { get; private set; } = new[]
    { 
        StatusIdConst.FORMED, StatusIdConst.CANCELED
    };

    public const int _0 = 0;
    public const int _1 = 1;
    public const int _7 = 7;
    public const int _12 = 12;
    public const int _31 = 31;
}