using System;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class SrvYearlyPlanTableDistrictDlDto : EntityDto<SrvYearlyPlanTableDistrictDlDto, SrvYearlyPlanTableDistrict>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int MonthOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }
    [LocalizedRequired]
    public decimal Amount { get; set; }
}
