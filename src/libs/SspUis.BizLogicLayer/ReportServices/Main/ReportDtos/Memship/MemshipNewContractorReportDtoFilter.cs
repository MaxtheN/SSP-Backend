using System;


namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipNewContractorReportDtoFilter
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;

    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
}
