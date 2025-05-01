using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppDtoSortFilter : SortFilterPageOptions
    {
        public int? TableId { get; set; }
        public int? AppId { get; set; }
    }
}
