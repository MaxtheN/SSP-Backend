using WEBASE.Models;
namespace SspUis.BizLogicLayer.Hrm;

public class OrderToSendBusinessTripSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
}
