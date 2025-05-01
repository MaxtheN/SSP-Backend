using Hangfire.Annotations;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class DocumentChatListDtoSortFilterPageOption : SortFilterPageOptions
    {
        [LocalizationRequired]
        public int TableId { get; set; }
        [LocalizationRequired]
        public long DocumentId { get; set; }
    }
}