using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Appeal;

public class AppealApplicationDlDto<TDto> : EntityDto<TDto, AppealApplication>
    where TDto : AppealApplicationDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string PhoneNumber { get; set; }
    [LocalizedStringLength(500)]
    public string PersonFullName { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(2500)]
    public string Details { get; set; }
    public long? ContractorId { get; set; }
    public int? PersonId { get; set; }
    [LocalizedRequired]
    public bool Busyness { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int AppealTypeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int AppealFormatTypeId { get; set; }
    [LocalizedRequired]
    public bool OpenAppeal { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RegionId { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int? AppealTypeArriveId { get; set; }
    public int? AppealDescriptionId { get; set; }
    public int? DepartmentId { get; set; }
    [LocalizedStringLength(250)]
    public string? Email { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string Address { get; set; }
    public bool IsCreatedByChamber { get; set; } = false;
    public int? OrganizationId { get; set; }

    public List<AppealApplicationFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, AppealApplication>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Files, c => c.Ignore());


    public override AppealApplication CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_APPEAL_APPLICATION, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(AppealApplication entity)
    {
        base.UpdateEntity(entity);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_APPEAL_APPLICATION, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        if (entity.StatusId != StatusIdConst.IN_EXECUTION
         && entity.StatusId != StatusIdConst.EXECUTED)
            entity.StatusId = StatusIdConst.MODIFIED;
    }
}
