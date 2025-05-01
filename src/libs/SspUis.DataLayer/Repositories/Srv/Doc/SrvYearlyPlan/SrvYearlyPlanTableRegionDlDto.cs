using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class SrvYearlyPlanTableRegionDlDto : EntityDto<SrvYearlyPlanTableRegionDlDto, SrvYearlyPlanTableRegion>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int MonthOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    [LocalizedRequired]
    public decimal Amount { get; set; }
    public List<SrvYearlyPlanTableDistrictDlDto> Districts { get; set; }
    protected override Action<IMappingExpression<SrvYearlyPlanTableRegionDlDto, SrvYearlyPlanTableRegion>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Districts, c => c.Ignore());
}
