using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public interface IMemshipApplicationRepository : IBaseApplicationRepository<long, MemshipApplication, CreateMemshipApplicationDlDto, UpdateMemshipApplicationDlDto, UpdateStatusMemshipApplicationDlDto>
{
}
