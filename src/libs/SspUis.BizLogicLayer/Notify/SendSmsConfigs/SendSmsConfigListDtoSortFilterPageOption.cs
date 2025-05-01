using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsConfigListDtoSortFilterPageOption : SortFilterPageOptions
    {
        public int? TableId { get; set; }
        public int? FromStatusId { get; set; }
        public int? ToStatusId { get; set; }
    }
}
