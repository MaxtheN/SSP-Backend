using WEBASE.Models;
namespace SspUis.BizLogicLayer.Hrm;

public class MassPlannedCalculationSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
    public int? RoundingTypeId { get; set; } 
    public int? OrgSettlementAccountId { get; set; }
    public int? CalculationKindId { get; set; } 
}
