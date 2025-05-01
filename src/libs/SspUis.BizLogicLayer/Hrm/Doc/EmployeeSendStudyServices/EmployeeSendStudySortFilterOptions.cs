using WEBASE.Models;
namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendStudySortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
