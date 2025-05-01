using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SrvYearlyPlanDlDto<TDto> : EntityDto<TDto, SrvYearlyPlan>
    where TDto : SrvYearlyPlanDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string? Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Year { get; set; }
    public List<SrvYearlyPlanTableCellDto> CellTables { get; set; }
    [JsonIgnore]
    public List<SrvYearlyPlanTableRegionDlDto> Regions { get; set; }
    public List<SrvYearlyPlanFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, SrvYearlyPlan>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Regions, c => c.Ignore()).ForMember(x => x.Files, c => c.Ignore());
    public override SrvYearlyPlan CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;

        Regions.AddTo(entity.Regions, (e, d) => d.Districts.AddTo(e.Districts));

        entity.Files.AddFromTempFiles(
        DocumentStorageConst.DOC_SRV_YEARLY_PLAN_FILES,
        Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(SrvYearlyPlan entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        
        Regions.ApplyChangesTo<long, SrvYearlyPlanTableRegionDlDto, SrvYearlyPlanTableRegion>(entity.Regions, (e, d) => d.Districts.ApplyChangesTo<long, SrvYearlyPlanTableDistrictDlDto, SrvYearlyPlanTableDistrict>(e.Districts));
  
        entity.Files.AddFromTempFiles(
               DocumentStorageConst.DOC_SRV_YEARLY_PLAN_FILES,
               Files.Select(a => a.Id).ToList());
    }
    public void BeforeSave()
    {
        Regions = CellTables.SelectMany(a => a.MonthVsValues.SelectMany(b => b.RegionValues.Select(c => new SrvYearlyPlanTableRegionDlDto
        {
            RegionId = b.RegionId,
            MonthOn = c.MonthOn,
            Amount = c.Amount,
            Districts = CellTables.SelectMany(d => d.MonthVsValues.SelectMany(g => g.ColumnForDistricts.SelectMany(k => k.DistrictValues.Select(f => new SrvYearlyPlanTableDistrictDlDto 
            {
                DistrictId = k.DistrictId,
                MonthOn = f.MonthOn,
                Amount = f.Amount,
            })))).ToList()
        }))).ToList();
    }
}
