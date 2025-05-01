using System;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class SrvYearlyPlanTableCellDto
{
    public List<MonthColumn> MonthColumns { get; set; } = new();
    public List<MonthVsValue> MonthVsValues { get; set; } = new();
}

public class MonthVsValue
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    public string Region { get; set; }
    public List<RegionValue> RegionValues { get; set; } = new();
    public List<ColumnForDistrict> ColumnForDistricts { get; set; } = new();
}
public class ColumnForDistrict
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }
    public string District { get; set; }
    public List<DistrictValue> DistrictValues { get; set; } = new();
}
public class MonthColumn
{
    public int? MonthOn { get; set; }
    public string MonthName { get; set; }
}

public class RegionValue
{
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int MonthOn { get; set; }
    public int Amount { get; set; }
   // public bool IsKvartal { get; set; }
}
public class DistrictValue
{
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int MonthOn { get; set; }
    public int Amount { get; set; }
    //public bool IsKvartal { get; set; }
}

