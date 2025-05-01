using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipNewContractorReportDto
{
    public int? RegionId { get; set; }
    public int DistrictId { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public int TotalLegalCount { get; set; }
    public int TotalPhysicalCount { get; set; }
    public int TotalLegalMemshipCount { get; set; }
    public int TotalPhysicalMemshipCount { get; set; }
    public int RatingScore { get; set; }
    public string Rating { get; set; }
}

public class NewContractorDto
{
    public int NewLegalContractorCount { get; set; }
    public int NewPhysicalContractorCount { get; set; }

}

public class MemshipContractorDto
{
    public int LegalMemshipContractorCount { get; set; }
    public int PhysicalMemshipContractorCount { get; set; }

}