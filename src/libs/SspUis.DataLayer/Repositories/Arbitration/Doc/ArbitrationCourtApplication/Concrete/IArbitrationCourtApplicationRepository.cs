using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public interface IArbitrationCourtApplicationRepository
    : IBaseApplicationRepository<long,
        ArbitrationCourtApplication,
        CreateArbitrationCourtApplicationDlDto,
        UpdateArbitrationCourtApplicationDlDto,
        UpdateStatusArbitrationCourtApplicationDlDto,
        UpdateStepArbitrationCourtApplicationDlDto>
{
}
