using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateStatusApplicationForCourtDlDto
 : EntityDto<UpdateStatusApplicationForCourtDlDto, ApplicationForCourt>
 , IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int StatusId { get; set; }
    public string Message { get; set; }
}
public class UpdateStepApplicationForCourtDlDto : EntityDto<UpdateStepApplicationForCourtDlDto, ApplicationForCourt>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int StepId { get; set; }

    public override void UpdateEntity(ApplicationForCourt entity)
    {
        base.UpdateEntity(entity);
        entity.StepId = StepId;
    }
}

public class AcceptUpdateStatusApplicationForCourtDlDto
{
    [LocalizedRequired, LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }

    [LocalizedRequired]
    public string SignedData { get; set; }
    public Guid DataFile { get; set; }
    public Guid SignFile { get; set; }
    public string SignedUserInfo { get; set; }
    public bool IsPinfl { get; set; } = false;
}
