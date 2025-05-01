using GenericServices;
using SspUis.DataLayer.Repositories.Claim;

namespace SspUis.BizLogicLayer.Claim
{
    public class UpdateStatusMediationPlanDto : UpdateStatusMediationPlanDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        public string? Message { get; set; }
    }
}
