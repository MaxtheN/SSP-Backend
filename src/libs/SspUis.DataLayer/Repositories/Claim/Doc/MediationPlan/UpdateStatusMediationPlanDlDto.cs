using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Claim;

public class UpdateStatusMediationPlanDlDto : EntityDto<UpdateStatusMediationPlanDlDto, MediationPlan>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int StatusId { get; set; }
}
