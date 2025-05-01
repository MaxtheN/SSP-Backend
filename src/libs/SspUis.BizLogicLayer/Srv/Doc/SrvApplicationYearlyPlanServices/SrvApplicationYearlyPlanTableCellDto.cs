using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanTableCellDto
{
    public int RegionId { get; set; }
    public string Region { get; set; }
    public int? RegionFreeCount { get; set; }
    public int? RegionPaidCount { get; set; }
    public decimal? RegionAmount { get; set; }
    public int? RegionEmployeeCount { get; set; }
    public decimal? RegionLegalAmount { get; set; }
    public decimal? RegionEconomyAmount { get; set; }
    public List<ValueForDistrict> ValueForDistricts { get; set; } = new();
}
public class ValueForDistrict
{
    public int DistrictId { get; set; }
    public string District { get; set; }
    public int? FreeCount { get; set; }
    public int? PaidCount { get; set; }
    public decimal? Amount { get; set; }
    public int? EmployeeCount { get; set; }
    public decimal? LegalAmount { get; set; }
    public decimal? EconomyAmount { get; set; }
}

