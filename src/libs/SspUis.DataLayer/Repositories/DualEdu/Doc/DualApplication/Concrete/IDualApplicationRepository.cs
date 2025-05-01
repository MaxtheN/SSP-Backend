using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.DataLayer.Repositories;

public interface IDualApplicationRepository
        : IBaseApplicationRepository<long, DualApplication, CreateDualApplicationDlDto, UpdateDualApplicationDlDto, UpdateStatusDualApplicationDlDto>
{
}
