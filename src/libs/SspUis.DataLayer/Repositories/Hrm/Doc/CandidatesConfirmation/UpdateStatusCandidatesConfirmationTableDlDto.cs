using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusCandidatesConfirmationTableDlDto
        : EntityDto<UpdateStatusCandidatesConfirmationTableDlDto, CandidatesConfirmationTable>
        , IHaveIdProp<long>
        , IHaveStatusId
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int StatusId { get; set; }
    }
}