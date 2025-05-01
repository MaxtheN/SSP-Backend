using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipYearlyPlanDlDto<TDto> : EntityDto<TDto, MemshipYearlyPlan>
    where TDto : MemshipYearlyPlanDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string? Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Year { get; set; }
    [LocalizedRequired]
    public bool IsRegion { get; set; }
    public List<MemshipYearlyPlanTableCellDto> CellTables { get; set; }
    [JsonIgnore]
    public List<MemshipYearlyPlanTableDlDto> Tables { get; set; }
    public List<MemshipYearlyPlanFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, MemshipYearlyPlan>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore()).ForMember(x => x.Files, c => c.Ignore());
    public override MemshipYearlyPlan CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.MemshipContractTypeId = MemshipContractTypeIdConst.FREE;
        Tables.AddTo(entity.Tables);
        entity.Files.AddFromTempFiles(
        DocumentStorageConst.DOC_MEMSHIP_YEARLY_PLAN_FILES,
        Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(MemshipYearlyPlan entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        entity.MemshipContractTypeId = MemshipContractTypeIdConst.FREE;
        Tables.ApplyChangesTo<long, MemshipYearlyPlanTableDlDto, MemshipYearlyPlanTable>(entity.Tables);
        entity.Files.AddFromTempFiles(
               DocumentStorageConst.DOC_MEMSHIP_YEARLY_PLAN_FILES,
               Files.Select(a => a.Id).ToList());
    }
    public void BeforeSave()
    {
        Tables = CellTables.SelectMany(a => a.MonthVsValues.SelectMany(b => b.Values.Where(c => !c.IsKvartal).Select(c => new MemshipYearlyPlanTableDlDto
        {
            RegionId = b.RegionId,
            DistrictId = b.DistrictId,
            MonthOn = c.MonthOn,
            MembersCount = c.MembersCount
        }))).ToList();
    }
}
