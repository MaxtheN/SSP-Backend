using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ISubsidyRequestRepository
        : IBaseEntityRepository<long, SubsidyRequest, CreateSubsidyRequestDlDto, UpdateSubsidyRequestDlDto, UpdateStatusSubsidyRequestDlDto>
    {
    }
}
