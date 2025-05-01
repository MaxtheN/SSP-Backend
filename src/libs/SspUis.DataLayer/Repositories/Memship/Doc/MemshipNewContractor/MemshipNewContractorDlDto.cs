
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MemshipNewContractorDlDto<TDto> : EntityDto<TDto, MemshipNewContractor>
    where TDto : MemshipNewContractorDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string? Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }

    [LocalizedRequired]
    public DateOnly FromDate { get; set; }
    [LocalizedRequired]
    public DateOnly ToDate { get; set; }
    [LocalizedRequired]

    [LocalizedRange(0, int.MaxValue)]
    public int TotalLegalCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int TotalPhysicalCount { get; set; }
    

    [JsonIgnore]
    public List<MemshipNewContractorTableDlDto> Tables { get; set; }
    public List<MemshipNewContractorFileDlDto> Files { get; set; } = new();
    protected override Action<IMappingExpression<TDto, MemshipNewContractor>> AlterMapping =>
      cfg => cfg.ForMember(x => x.Tables, c => c.Ignore()).ForMember(x => x.Files, c => c.Ignore());
    public override MemshipNewContractor CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        
        Tables.AddTo(entity.Tables);
        entity.Files.AddFromTempFiles(
        DocumentStorageConst.DOC_MEMSHIP_NEW_CONTRACTOR,
        Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(MemshipNewContractor entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        
        Tables.ApplyChangesTo<long, MemshipNewContractorTableDlDto, MemshipNewContractorsTable>(entity.Tables);
        entity.Files.AddFromTempFiles(
               DocumentStorageConst.DOC_MEMSHIP_NEW_CONTRACTOR,
               Files.Select(a => a.Id).ToList());
    }
    
}
