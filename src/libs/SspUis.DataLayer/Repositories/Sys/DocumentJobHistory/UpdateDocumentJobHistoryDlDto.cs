using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateDocumentJobHistoryDlDto : DocumentJobHistoryDlDto<UpdateDocumentJobHistoryDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
    }
}
