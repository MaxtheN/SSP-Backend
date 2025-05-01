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

public class SrvApplicationYearlyPlanDlDto<TDto> : EntityDto<TDto, SrvApplicationYearlyPlan>
    where TDto : SrvApplicationYearlyPlanDlDto<TDto>
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
    [LocalizedRange(1, int.MaxValue)]
    public int MonthOn { get; set; }
    [JsonIgnore]
    public int RegionId { get; set; }
    [JsonIgnore]
    public int RegionFreeCount { get; set; }
    [JsonIgnore]
    public int RegionPaidCount { get; set; }
    [JsonIgnore]
    public int RegionEmployeeCount { get; set; }
    [JsonIgnore]
    public decimal RegionAmount { get; set; }
    [JsonIgnore]
    public decimal RegionLegalAmount { get; set; }
    [JsonIgnore]
    public decimal RegionEconomyAmount { get; set; }
    public List<SrvApplicationYearlyPlanTableCellDto> CellTables { get; set; }
    [JsonIgnore]
    public List<SrvApplicationYearlyPlanTableDlDto> Tables { get; set; }
    public List<SrvApplicationYearlyPlanFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, SrvApplicationYearlyPlan>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore()).ForMember(x => x.Files, c => c.Ignore());
    public override SrvApplicationYearlyPlan CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        entity.Files.AddFromTempFiles(
        DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES,
        Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(SrvApplicationYearlyPlan entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long, SrvApplicationYearlyPlanTableDlDto, SrvApplicationYearlyPlanTable>(entity.Tables);
        entity.Files.AddFromTempFiles(
               DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES,
               Files.Select(a => a.Id).ToList());
    }
    public void BeforeSave()
    {
        RegionId = CellTables.FirstOrDefault().RegionId;
        RegionFreeCount = CellTables.FirstOrDefault().RegionFreeCount;
        RegionPaidCount = CellTables.FirstOrDefault().RegionPaidCount;
        RegionEmployeeCount = CellTables.FirstOrDefault().RegionEmployeeCount;
        RegionAmount = CellTables.FirstOrDefault().RegionAmount;
        RegionLegalAmount = CellTables.FirstOrDefault().RegionLegalAmount;
        RegionEconomyAmount = CellTables.FirstOrDefault().RegionEconomyAmount;
        Tables = CellTables.SelectMany(distrct => distrct.ValueForDistricts.Select(distrctItem => new SrvApplicationYearlyPlanTableDlDto
        {
            DistrictId = distrctItem.DistrictId,
            FreeCount = distrctItem.FreeCount,
            PaidCount = distrctItem.PaidCount,
            EmployeeCount = distrctItem.EmployeeCount,
            Amount = distrctItem.Amount,
            LegalAmount = distrctItem.LegalAmount,
            EconomyAmount = distrctItem.EconomyAmount
        })).ToList();
    }
}
