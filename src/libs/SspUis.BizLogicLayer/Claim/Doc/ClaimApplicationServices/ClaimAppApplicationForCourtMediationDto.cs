using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.ClaimApplicationServices;

namespace SspUis.BizLogicLayer;

public class ClaimAppApplicationForCourtMediationDto
{
    public ClaimApplicationDto ClaimApplication { get; set; } = new();
    public ApplicationForCourtDto ApplicationForCourt { get; set; } = new();
	public MediationDto Mediation { get; set; } = new();
    public MediationPlanDto MediationPlan { get; set; } = new();
}
