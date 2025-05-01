using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateProposalDlDto : ProposalDlDto<UpdateProposalDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
    }
}
