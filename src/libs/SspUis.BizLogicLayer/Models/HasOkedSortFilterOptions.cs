using WEBASE;

namespace SspUis.BizLogicLayer.Models
{
    public class HasOkedSortFilterOptions : TableSortFilterPageOptions
    {
        public bool ByOked { get; set; } = false;
        public int? ParentOrganizationId { get; set; }
        public string OkedCode { get; set; }
    }
}
