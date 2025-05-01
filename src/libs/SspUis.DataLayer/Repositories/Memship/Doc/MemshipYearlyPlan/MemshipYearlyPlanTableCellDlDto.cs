using System;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipYearlyPlanTableCellDto
{
    public List<MonthColumn> MonthColumns { get; set; } = new();
    public List<MonthVsValue> MonthVsValues { get; set; } = new();
}

public class MonthVsValue
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    public int? DistrictId { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public List<Value> Values { get; set; } = new();
}

public class MonthColumn
{
    public int? MonthOn { get; set; }
    public string MonthName { get; set; }
}

public class Value
{
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int MonthOn { get; set; }
    public int MembersCount { get; set; }
    public bool IsKvartal { get; set; }
}

