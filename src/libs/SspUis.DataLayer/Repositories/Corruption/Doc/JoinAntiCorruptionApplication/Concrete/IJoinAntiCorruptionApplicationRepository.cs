using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public interface IJoinAntiCorruptionApplicationRepository 
    : IBaseApplicationRepository<long, JoinAntiCorruptionApplication, CreateJoinAntiCorruptionApplicationDlDto, 
    UpdateJoinAntiCorruptionApplicationDlDto, UpdateStatusJoinAntiCorruptionApplicationDlDto>
{
    void UpdateStep(UpdateStepDlDto dto);
}
