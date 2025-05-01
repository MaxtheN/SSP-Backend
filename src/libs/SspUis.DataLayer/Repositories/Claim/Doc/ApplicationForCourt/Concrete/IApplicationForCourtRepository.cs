using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IApplicationForCourtRepository : IBaseEntityRepository<long, ApplicationForCourt, CreateApplicationForCourtDlDto, UpdateApplicationForCourtDlDto,UpdateStatusApplicationForCourtDlDto>
    {
        void UpdateStep(ApplicationForCourt entity, UpdateStepApplicationForCourtDlDto dto);
    }
}
