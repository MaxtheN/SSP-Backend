using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;

public class JoinAntiCorruptionResultDlDto<TDto> : EntityDto<TDto, JoinAntiCorruptionResult>
    where TDto : JoinAntiCorruptionResultDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int ChairmenId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ChairmenOrganizationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ChairmenPositionId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string ChairmenFio { get; set; }
    public int Member1Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Member1OrganizationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Member1PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string Member1Fio { get; set; }
    public int? Member2Id { get; set; }
    public int? Member2OrganizationId { get; set; }
    public int? Member2PositionId { get; set; }
    [LocalizedStringLength(250)]
    public string Member2Fio { get; set; }
    public int? Member3Id { get; set; }
    public int? Member3OrganizationId { get; set; }
    public int? Member3PositionId { get; set; }
    [LocalizedStringLength(250)]
    public string Member3Fio { get; set; }
    public int? Member4Id { get; set; }
    public int? Member4OrganizationId { get; set; }
    public int? Member4PositionId { get; set; }
    [LocalizedStringLength(250)]
    public string Member4Fio { get; set; }
    public List<JoinAntiCorruptionResultTableDlDto> Tables { get; set; } = new();
    public List<JoinAntiCorruptionResultFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, JoinAntiCorruptionResult>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, c => c.Ignore()).ForMember(x => x.Files, c => c.Ignore());

    public override JoinAntiCorruptionResult CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        entity.Files.AddFromTempFiles(
                DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_RESULT_FILES,
                Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(JoinAntiCorruptionResult entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long, JoinAntiCorruptionResultTableDlDto, JoinAntiCorruptionResultTable>(entity.Tables);
        entity.Files.AddFromTempFiles(
                DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_RESULT_FILES,
                Files.Select(a => a.Id).ToList());
    }
}
