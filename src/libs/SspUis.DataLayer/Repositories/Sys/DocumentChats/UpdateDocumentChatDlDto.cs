using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateDocumentChatDlDto : DocumentChatDlDto<UpdateDocumentChatDlDto>,IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int StateId { get; set; }
    }
}
