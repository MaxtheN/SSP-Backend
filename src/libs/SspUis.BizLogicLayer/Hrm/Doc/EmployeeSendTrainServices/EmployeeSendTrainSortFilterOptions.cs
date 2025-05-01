using WEBASE.Models;
namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendTrainSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
