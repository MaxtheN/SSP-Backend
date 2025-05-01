using WEBASE.Models;
namespace SspUis.BizLogicLayer.Claim;

public class MediationPlanSortFilterOptions : SortFilterPageOptions
{
    public bool IsEmployee { get; set; } = false;
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
    public int? ContractorId { get; set; }
}
