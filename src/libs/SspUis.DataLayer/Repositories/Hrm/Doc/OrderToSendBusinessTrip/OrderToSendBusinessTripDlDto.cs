using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class OrderToSendBusinessTripDlDto<TDto> : EntityDto<TDto, OrderToSendBusinessTrip>
    where TDto : OrderToSendBusinessTripDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(30)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedStringLength(600)]
    public string? Details { get; set; }
    public string ConclusionForPrint { get; set; }
    public int? OrganizationId { get; set; }
    public List<OrderToSendBusinessTripTableDlDto> Tables { get; set; }
    public List<OrderToSendBusinessTripSignerDlDto> Signer { get; set; }
    public virtual List<OrderToSendBusinessTripFileDlDto> Files { get; set; } = new();

    [LocalizedRequired]
    public DateOnly? WorkStarDate { get; set; }

    protected override Action<IMappingExpression<TDto, OrderToSendBusinessTrip>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override OrderToSendBusinessTrip CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_EMPLOYEE_SEND_BUSINESS_TRIP, Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(OrderToSendBusinessTrip entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long,OrderToSendBusinessTripTableDlDto,OrderToSendBusinessTripTable>(entity.Tables);
        Signer.ApplyChangesTo<long, OrderToSendBusinessTripSignerDlDto, OrderToSendBusinessTripSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_EMPLOYEE_SEND_BUSINESS_TRIP, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
