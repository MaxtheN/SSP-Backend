using WEBASE.Models;
namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
