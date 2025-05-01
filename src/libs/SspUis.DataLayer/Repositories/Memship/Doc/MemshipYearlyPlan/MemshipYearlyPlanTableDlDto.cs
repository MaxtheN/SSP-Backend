using System;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipYearlyPlanTableDlDto : EntityDto<MemshipYearlyPlanTableDlDto, MemshipYearlyPlanTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int MonthOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    public int? DistrictId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int MembersCount { get; set; }
}
