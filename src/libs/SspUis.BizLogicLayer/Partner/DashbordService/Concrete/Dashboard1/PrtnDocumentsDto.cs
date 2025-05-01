using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Partner;

public class PrtnDocumentsDto
{
    public int? PrtnContractTypeId { get; set; } 
    public string PrtnContractType { get; set; } 
    public int? RegionId { get; set; }
    public string Region { get; set; }
    public int? DistrictId { get; set; }
    public string District { get; set; }
    public long? MfyId { get; set; }
    public string Mfy { get; set; }
}

public class PrtnDocumentsRegionRate
{
    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }
    public int? DistrictId { get; set; }
    public string DistrictOrderCode { get; set; }
    public long? MfyId { get; set; }
    public string MfyOrderCode { get; set; }
    public int DocCount { get; set; }
}


public class PrtnCertificateCountDto : PrtnDocumentsDto
{
    public int FormedCount { get; set; }
    public int CancelCount { get; set; }
}

public class PrtnContractCountDto : PrtnDocumentsDto
{
    public int DocCount { get; set; }
    public int StatusId { get; set; }
    public string? Status { get; set; }
}

public class PrtnApplicationCountDto : PrtnContractCountDto 
{ 

}

public class PrtnStatisticsDto
{
    public int TotalApplicationsReceivedCount { get; set; }
    public int TotalApplicationsReviewCount { get; set; }
    public int TotalApplicationsRejectedCount { get; set; }
    //public int ContractsExecutingCount { get; set; }
    public int ContractsExpertiseCount { get; set; }
    public int ContractsSingingCount { get; set; }
    public int ContractsSignedCount { get; set; }
    public int CertificateFormedCount { get; set; }
    public int VacanciesCount { get; set; }
}

public class PrtnContractTypeDto
{
    public int PrtnContractTypeId { get; set; }
    public string PrtnContractType { get; set; }
    public List<PrtnContractColumn> PrtnApplication { get; set; } = new();
    public List<PrtnContractColumn> PrtnContract { get; set; } = new();
    public List<PrtnContractColumn> PrtnCertificate { get; set; } = new();
}


//public class PrtnContractRegionTypeDto
//{
//    public int PrtnContractTypeId { get; set; }
//    public string PrtnContractType { get; set; }
//    public List<PrtnDocumentsContractRegionRate> PrtnApplication { get; set; } = new();
//    public List<PrtnDocumentsContractRegionRate> PrtnContract { get; set; } = new();
//    public List<PrtnDocumentsContractRegionRate> PrtnCertificate { get; set; } = new();
//}

//public class PrtnDocumentsContractRegionRate
//{
//    public int? RegionId { get; set; }
//    public string RegionOrderCode { get; set; }
//    public string Region { get; set; }
//    public int? DistrictId { get; set; }
//    public string DistrictOrderCode { get; set; }
//    public long? MfyId { get; set; }
//    public string MfyOrderCode { get; set; }
//    public long DocCount { get; set; }
//}

public class PrtnContractColumn
{
    public int ContractorCount { get; set; }
    public int VacancyCount { get; set; }
}