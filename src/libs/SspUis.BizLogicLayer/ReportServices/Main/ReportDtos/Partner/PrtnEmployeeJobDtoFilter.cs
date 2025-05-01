using System;

namespace SspUis.BizLogicLayer.ReportServices;

public class PrtnEmployeeJobDtoFilter
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public int? PrtnContractTypeId { get; set; }

    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;

    public long? ContractorId { get; set; }
    public bool ByContractor { get; set; } = false;
}