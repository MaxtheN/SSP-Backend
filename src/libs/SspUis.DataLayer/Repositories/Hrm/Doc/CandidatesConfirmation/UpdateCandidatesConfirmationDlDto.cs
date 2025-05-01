using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateCandidatesConfirmationDlDto
        : CandidatesConfirmationDlDto<UpdateCandidatesConfirmationDlDto>,
        IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }
    }
}