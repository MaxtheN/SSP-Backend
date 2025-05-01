using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MemshipCertificateDlDto<TDto> : EntityDto<TDto, MemshipCertificate>
    where TDto : MemshipCertificateDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedRequired]
    public DateOnly ExpireOn { get; set; }
    public DateOnly? CancelOn { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public bool IsRead { get; set; }
    public long MemshipContractId { get; set; }
	public string? Details { get; set; }
	public long? ContractorSettlementAccountId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long ContractorId { get; set; }
    public string Message { get; set; }
    public DateOnly? CancelDay { get; set; }
    public string CancelReasen { get; set; }
    public List<MemshipCertificateFileDlDto> Files { get; set; } = new();
    protected override Action<IMappingExpression<TDto, MemshipCertificate>> AlterMapping =>
        cfg => cfg
            .ForMember(x => x.Files, x => x.Ignore());

    public override MemshipCertificate CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.Id2 = Guid.NewGuid();
        entity.IsRead = false;
        entity.StatusId = StatusIdConst.FORMED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES, Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(MemshipCertificate entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_MEMSHIP_CERTIFICATE_FILES, entity.Id.ToString(), Files.Select(x => x.Id).ToList());
    }
}