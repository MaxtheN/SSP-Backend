using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Memship;

[Keyless]
public class MemshipReportResult
{

    [Column("application_accepted_count")]
    public long MemshipApplicationAcceptedCount { get; set; }
    [Column("application_review_count")]
    public long MemshipApplicationReviewCount { get; set; }
    [Column("application_reject_count")]
    public long MemshipApplicationRejectedCount { get; set; }

    [Column("contract_accepted_count")]
    public long MemshipContractAcceptedCount { get; set; }
    [Column("contract_review_count")]
    public long MemshipContractReviewCount { get; set; }
    [Column("contract_rejected_count")]
    public long MemshipContractRejectedCount { get; set; }
    [Column("certificate_review_count")]

    public long MemshipCertificateReviewCount { get; set; }
    [Column("certificate_formed_count")]
    public long MemshipCertificateFormedCount { get; set; }

    [Column("total_application_count")]
    public long TotalMemshipApplicationCount { get; set; }
    [Column("total_contract_count")]
    public long TotalMemshipContractCount { get; set; }
    [Column("certificate_formed_count")]
    public long TotalMemshipCertificateCount { get; set; }
}

public class MemshipDocsInfoDto : MemshipReportResult
{
    [Column("region")]
    public string Region { get; set; }
    [Column("region_id")]
    public int? RegionId { get; set; }

    [Column("district")]
    public string District { get; set; }
    [Column("district_id")]
    public int? DistrictId { get; set; }
}

public class MemshipDocsInfoReestrModelDto : MemshipReportResult
{
    [Column("id")]
    public long? ContractorId { get; set; }
    [Column("full_name")]
    public string Contractor { get; set; }
    [Column("inn")]
    public string ContractorInn { get; set; }
}
public class MemshipDocsInfoReestrDto
{
    public List<MemshipDocsInfoReestrModelDto> Rows { get; set; }
    public long Count { get; set; }
}
[Keyless]
public class CountResult
{
    [Column("total_count")]
    public long TotalCount { get; set; }
}
