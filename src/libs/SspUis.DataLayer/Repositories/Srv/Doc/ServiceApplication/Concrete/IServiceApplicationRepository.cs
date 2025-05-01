using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public interface IServiceApplicationRepository 
        : IBaseApplicationRepository<long, ServiceApplication, CreateServiceApplicationDlDto, UpdateServiceApplicationDlDto, UpdateStatusServiceApplicationDlDto>
    {
    }
}
