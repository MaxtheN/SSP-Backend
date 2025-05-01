using SspUis.Core;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Claim;

public class MediationPlanDlDto<TDto> : EntityDto<TDto, MediationPlan>
    where TDto : MediationPlanDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; } = null!;
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long ApplicationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int MeetingTypeId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string AddressOrUrl { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long ContractorId { get; set; }
    [LocalizedRequired]
    public DateTime MeditionAt { get; set; } = DateTime.Now;
    [LocalizedRequired]
    [LocalizedStringLength(200)]
    public string ChamberPerson { get; set; }

    public override MediationPlan CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Id2 = Guid.NewGuid();
        return entity;
    }
    public override void UpdateEntity(MediationPlan entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
