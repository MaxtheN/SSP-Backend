using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.DataLayer.Repositories
{
    public interface IClaimApplicationRepository
        : IBaseApplicationRepository<long, ClaimApplication, CreateClaimApplicationDlDto, UpdateClaimApplicationDlDto, UpdateStatusClaimApplicationDlDto>
    {
        void UpdateEmployeeAttachment(UpdateEmployeeAttechmentDlDto dto);
        void UpdateStep(UpdateStepDlDto dto);
    }
}
