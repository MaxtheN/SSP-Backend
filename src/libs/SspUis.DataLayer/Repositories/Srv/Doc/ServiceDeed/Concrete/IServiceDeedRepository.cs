using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IServiceDeedRepository
        : IBaseEntityRepository<long, ServiceDeed, CreateServiceDeedDlDto, UpdateServiceDeedDlDto, UpdateStatusServiceDeedDlDto>
    {
    }
}
