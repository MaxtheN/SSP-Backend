using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class CallCenterAppealDlDto<TDto> : EntityDto<TDto, CallCenterAppeal>
    where TDto : CallCenterAppealDlDto<TDto>
{
    public string? DocNumber { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string PhoneNumber { get; set; }
    [LocalizedStringLength(500)]
    public string PersonFullName { get; set; }
    [LocalizedStringLength(150)]
    public string? Summary { get; set; }
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
    public int? RegionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int? DistrictId { get; set; }
    public int? OkedId { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int? AppealTypeArriveId { get; set; }
    public int? AppealDescriptionId { get; set; }
    public int? DepartmentId { get; set; }
    public int? CountryId { get; set; }
    public bool Isimporter { get; set; } = false;
    public bool Isexporter { get; set; } = false;
    [LocalizedStringLength(250)]
    public string? Email { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string Address { get; set; }
    [LocalizedStringLength(50)]
    public string ContractorDirectorPinfl { get; set; }
    [LocalizedStringLength(50)]
    public string ContractorDirectorPassportSeria { get; set; }
    [LocalizedStringLength(50)]
    public string ContractorDirectorPassportNumber { get; set; }
    public DateTime? FcontractorDirectorBirthDate { get; set; }
    public int? ContractorCategoryId { get; set; }

    public List<CallCenterAppealFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, CallCenterAppeal>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Files, c => c.Ignore());


    public override CallCenterAppeal CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_CALL_CENTER_APPEAL, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(CallCenterAppeal entity)
    {
        base.UpdateEntity(entity);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_CALL_CENTER_APPEAL, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        if (entity.StatusId != StatusIdConst.IN_EXECUTION
            && entity.StatusId != StatusIdConst.EXECUTED)
            entity.StatusId = StatusIdConst.MODIFIED;
    }
}
