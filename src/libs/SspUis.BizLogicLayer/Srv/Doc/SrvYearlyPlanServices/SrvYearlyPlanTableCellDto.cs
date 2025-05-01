using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanTableCellDto
{
    public List<MonthColumn> MonthColumns { get; set; } = new();
    public List<MonthVsValue> MonthVsValues { get; set; } = new();
    public List<ColumnForRegion> ColumnForRegions { get; set; } = new();
}

public class MonthVsValue
{
    public int RegionId { get; set; }
    public string Region { get; set; }
    public List<RegionValue> RegionValues { get; set; } = new();
    public List<ColumnForDistrict> ColumnForDistricts { get; set; } = new();
}
public class ColumnForDistrict
{
    public int DistrictId { get; set; }
    public string District { get; set; }
    public List<DistrictValue> DistrictValues { get; set; } = new();
}
public class ColumnForRegion
{
    public int RegionId { get; set; }
    public string Region { get; set; }
    public List<RegionValue> RegionValues { get; set; } = new();
}
public class MonthColumn
{
    public int MonthOn { get; set; }
    public string MonthName { get; set; }
}

public class RegionValue
{
    public int? MonthOn { get; set; }
    public decimal? Amount { get; set; }
    //public bool IsKvartal { get; set; }
}
public class DistrictValue
{
    public int? MonthOn { get; set; }
    public decimal? Amount { get; set; }
    //public bool IsKvartal { get; set; }
}

