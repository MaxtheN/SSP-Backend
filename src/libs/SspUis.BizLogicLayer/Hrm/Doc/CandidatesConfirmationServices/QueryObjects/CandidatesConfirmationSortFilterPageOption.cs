using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationSortFilterPageOption : SortFilterPageOptions
    {
        public int? StatusId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
    }
}
