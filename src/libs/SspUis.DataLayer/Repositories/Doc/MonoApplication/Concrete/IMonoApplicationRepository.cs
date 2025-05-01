using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public interface IMonoApplicationRepository
        : IBaseApplicationRepository<long, MonoApplication, CreateMonoApplicationDlDto, UpdateMonoApplicationDlDto, UpdateStatusMonoApplicationDlDto>
    {

    }
}
