using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;

public class JoinAntiCorruptionCertificateDlDto<TDto> : EntityDto<TDto, JoinAntiCorruptionCertificate>
    where TDto : JoinAntiCorruptionCertificateDlDto<TDto>
{
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly? ExpireOn { get; set; }
    public DateOnly? CancelOn { get; set; }
    [LocalizedRequired]
    public long ContractorId { get; set; }
    [LocalizedRequired]
    public long ResultId { get; set; }
    public override JoinAntiCorruptionCertificate CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.FORMED;

        return entity;
    }

    public override void UpdateEntity(JoinAntiCorruptionCertificate entity)
    {
        base.UpdateEntity(entity);
    }
}
