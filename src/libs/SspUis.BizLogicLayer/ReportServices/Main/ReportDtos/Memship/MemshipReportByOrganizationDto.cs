using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship;

public class MemshipReportByOrganizationDto
{
    public int? RegionId { get; set; }
    public string Region { get; set; }
    public string RegionOrderCode { get; set; }
    public string DistrictOrderCode { get; set; }
    public int? DistrictId { get; set; }
    public string District { get; set;}
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public ApprovedApplication ApprovedApplication { get; set; }
    public SignedApplication SignedApplication { get; set; }
    public CertificateGivenApplication CertificateGivenApplication { get; set; }
}
public class ApprovedApplication
{
    public int TotalCount { get; set; }
    public int MicroOrgCount { get; set; }
    public int SmallOrgCount { get; set; }
    public int MiddleOrgCount { get; set; }
    public int BigOrgCount { get; set; }
}
public class SignedApplication
{
    public int TotalCount { get; set; }
    public int MicroOrgCount { get; set; }
    public int SmallOrgCount { get; set; }
    public int MiddleOrgCount { get; set; }
    public int BigOrgCount { get; set; }
}
public class CertificateGivenApplication
{
    public int TotalCount { get; set; }
    public int MicroOrgCount { get; set; }
    public int SmallOrgCount { get; set; }
    public int MiddleOrgCount { get; set; }
    public int BigOrgCount { get; set; }
}

public class Helpme
{
    public DateOnly DocOn { get; set; }
    public int? ApplicationRegId { get; set; }
    public int? CategoryId { get; set; }
    public int? ApplicationDisId { get; set; }
    public long? ContractorId { get; set; }
}
public class MemshipContractHelper
{
    public DateOnly DocOn { get; set; }
    public int? ApplicationRegId { get; set; }
    public int? ApplicationDisId { get; set; }
    public string ContractorPinfl { get; set; }
    public string ContractorInn { get; set; }
    public long? ContractorId { get; set; }
}
public class MemshipApplicationHelper
{
    public DateOnly DocOn { get; set; }
    public int? ApplicationRegId { get; set; }
    public int? ApplicationDisId { get; set; }
    public string ContractorPinfl { get; set; }
    public string ContractorInn { get; set; }
    public long? ContractorId { get; set; }
}
public class MemshipCertificateHelper
{
    public DateOnly DocOn { get; set; }
    public int? ApplicationRegId { get; set; }
    public int? ApplicationDisId { get; set; }
    public string ContractorPinfl { get; set; }
    public string ContractorInn { get; set; }
    public long? ContractorId { get; set; }
}
public class MemshipNewContractorHelper
{
   public int PhysicalPersonCount { get; set; }
   public int LegalPersonCount { get; set; }
   public int? ApplicationRegId { get; set; }
   public int? ApplicationDisId { get; set; }
}