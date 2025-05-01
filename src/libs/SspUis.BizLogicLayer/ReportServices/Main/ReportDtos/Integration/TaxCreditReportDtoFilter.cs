using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices.Main;

public class TaxCreditReportDtoFilter
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; }

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; }

    public long? ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public bool ByContractor { get; set; }
    public int? Year { get; set; }
    public int? LanguageId { get; set; }
    public int? ContarctTypeId { get; set; }
    public bool ByContactType { get; set; }
    public bool HasCertificate { get; set; } = true;
    public int? TaxCrediteType { get; set; }
    public int? Tab { get; set; }
}
public class TaxCreditReportDtoFilterPageOptions : SortFilterPageOptions
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; }

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; }

    public long? ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public bool ByContractor { get; set; }
    public int? Year { get; set; }
    public int? LanguageId { get; set; }
    public int? ContarctTypeId { get; set; }
    public bool ByContactType { get; set; }
    public bool HasCertificate { get; set; } = true;
    public int? TaxCrediteType { get; set; }
    public int? Tab { get; set; }
}