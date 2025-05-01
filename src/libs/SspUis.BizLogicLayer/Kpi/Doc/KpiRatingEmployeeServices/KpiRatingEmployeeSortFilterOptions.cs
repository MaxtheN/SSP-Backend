using WEBASE.Models;
namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeeSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
