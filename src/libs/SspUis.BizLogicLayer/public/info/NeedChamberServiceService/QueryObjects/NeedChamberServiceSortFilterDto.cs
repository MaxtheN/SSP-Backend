using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceSortFilterDto : SortFilterPageOptions
    {
        public bool? IsPaid { get; set; }
        public int? ServicePriceTypeId { get; set; }
        public int StateId { get; set; }
    }
}
