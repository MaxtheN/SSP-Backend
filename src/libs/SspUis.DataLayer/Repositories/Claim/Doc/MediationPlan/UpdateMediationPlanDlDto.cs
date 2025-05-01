using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Claim;

public class UpdateMediationPlanDlDto : MediationPlanDlDto<UpdateMediationPlanDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
