using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ICandidatesConfirmationRepository : IBaseEntityRepository<
            long,
            CandidatesConfirmation,
            CreateCandidatesConfirmationDlDto,
            UpdateCandidatesConfirmationDlDto,
            UpdateStatusCandidatesConfirmationDlDto>
    {

    }
}
