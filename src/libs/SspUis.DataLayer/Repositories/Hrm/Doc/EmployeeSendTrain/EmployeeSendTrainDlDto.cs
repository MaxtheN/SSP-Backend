using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSendTrainDlDto<TDto> : EntityDto<TDto, EmployeeSendTrain>
    where TDto : EmployeeSendTrainDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string ConclusionForPrint { get; set; }
    public int? OrganizationId { get; set; }
    [LocalizedRequired]
    public DateOnly? WorkStartDate { get; set; }
    public List<EmployeeSendTrainTableDlDto> Tables { get; set; }
    public List<EmployeeSendTrainSignerDlDto> Signer { get; set; }
    public virtual List<EmployeeSendTrainFileDlDto> Files { get; set; }

    protected override Action<IMappingExpression<TDto, EmployeeSendTrain>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override EmployeeSendTrain CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER, Files.Select(a => a.Id).ToList());

        return entity;
    }
    public override void UpdateEntity(EmployeeSendTrain entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long,EmployeeSendTrainTableDlDto,EmployeeSendTrainTable>(entity.Tables);
        Signer.ApplyChangesTo<long, EmployeeSendTrainSignerDlDto, EmployeeSendTrainSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
