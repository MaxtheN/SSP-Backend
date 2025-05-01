using GenericServices;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanTableCellDto
{
    public List<MonthColumn> MonthColumns { get; set; } = new();
    public List<MonthVsValue> MonthVsValues { get; set; } = new();
    public List<ColumnForRegion> ColumnForRegions { get; set; } = new();
}

public class MonthVsValue
{
    public int RegionId { get; set; }
    public int? DistrictId { get; set; }
    public string Region { get; set; }
    public string? District { get; set; }
    public int? TotalForYear { get; set; }
    public List<Value> Values { get; set; } = new();
}
public class ColumnForRegion
{
    public int RegionId { get; set; }
    public string Region { get; set; }
    public int? TotalForYear { get; set; }
    public List<Value> Values { get; set; } = new();
}

public class MonthColumn
{
    public int MonthOn { get; set; }
    public string MonthName { get; set; }
}

public class Value
{
    public int? MonthOn { get; set; }
    public int? MembersCount { get; set; }
    public bool IsKvartal { get; set; }
}

