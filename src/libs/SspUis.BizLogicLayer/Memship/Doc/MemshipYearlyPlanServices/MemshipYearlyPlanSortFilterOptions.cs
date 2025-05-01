using WEBASE.Models;
namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
