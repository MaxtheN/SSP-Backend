using System;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class SrvApplicationYearlyPlanTableCellDto
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionFreeCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionPaidCount { get; set; }
    [LocalizedRequired]
    public decimal RegionAmount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionEmployeeCount { get; set; }
    [LocalizedRequired]
    public decimal RegionLegalAmount { get; set; }
    [LocalizedRequired]
    public decimal RegionEconomyAmount { get; set; }
    public List<ValueForDistrict> ValueForDistricts { get; set; } = new();
}
public class ValueForDistrict
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int FreeCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int PaidCount { get; set; }
    [LocalizedRequired]
    public decimal Amount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeCount { get; set; }
    [LocalizedRequired]
    public decimal LegalAmount { get; set; }
    [LocalizedRequired]
    public decimal EconomyAmount { get; set; }
}

