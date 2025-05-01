using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class ChastisementSortFilter : SortFilterPageOptions
    {
        public int? EmployeeId { get; set; }
        public bool? IsSelectList { get; set; }
    }
}
