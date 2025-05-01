using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class JoinAntiCorruptionApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, JoinAntiCorruptionApplication>
    where TDto : JoinAntiCorruptionApplicationDlDto<TDto>
{
    public int CorruptionReviewTypeId { get; set; }
    public string Address { get; set; }
    public string Details { get; set; }
    public int ContractorActivityTypeId { get; set; }
    //public int ContractorUnionActivityTypeId { get; set; }
    public int UnionMemberCount { get; set; }
    public int AvgEmployeesCount { get; set; }
    public decimal PrevYearlyEarnings { get; set; }
    public int CurrencyId { get; set; }
    public List<JoinAntiCorruptionApplicationFileDlDto> Files { get; set; } = new();
    public List<JoinAntiCorruptionApplicationEmployeeDlDto> Employees { get; set; } = new();
    public List<JoinAntiCorruptionApplicationParticipateDlDto> Participates { get; set; } = new();
    public List<JoinAntiCorruptionApplicationTableDlDto> Tables { get; set; } = new();

    protected override Action<IMappingExpression<TDto, JoinAntiCorruptionApplication>> AlterMapping =>
            cfg =>
            {
                base.AlterMapping(cfg);
                cfg.ForMember(x => x.Files, x => x.Ignore());
                cfg.ForMember(x => x.Employees, x => x.Ignore());
                cfg.ForMember(x => x.Participates, x => x.Ignore());
                cfg.ForMember(x => x.Tables, x => x.Ignore());
            };

    public override JoinAntiCorruptionApplication CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.Application.Id2 = Guid.NewGuid();
        entity.Application.ApplicationTypeId = ApplicationTypeIdConst.CORRUPTION;
        entity.Application.StatusId = StatusIdConst.ACCEPTED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES, Files.Select(a => a.Id).ToList());
        Employees.AddTo(entity.Employees);
        Participates.AddTo(entity.Participates);
        Tables.AddTo(entity.Tables);
        return entity;
    }

    public override void UpdateEntity(JoinAntiCorruptionApplication entity)
    {
        base.UpdateEntity(entity);
        entity.Application.StatusId = StatusIdConst.MODIFIED;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        Employees.ApplyChangesTo<long, JoinAntiCorruptionApplicationEmployeeDlDto, JoinAntiCorruptionApplicationEmployee>(entity.Employees);
        Participates.ApplyChangesTo<long, JoinAntiCorruptionApplicationParticipateDlDto, JoinAntiCorruptionApplicationParticipate>(entity.Participates);
        Tables.ApplyChangesTo<long, JoinAntiCorruptionApplicationTableDlDto, JoinAntiCorruptionApplicationTable>(entity.Tables);
    }
}