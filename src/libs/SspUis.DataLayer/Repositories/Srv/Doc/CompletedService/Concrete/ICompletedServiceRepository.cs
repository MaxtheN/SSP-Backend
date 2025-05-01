using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ICompletedServiceRepository
        : IBaseEntityRepository<long, CompletedService, CreateCompletedServiceDlDto, UpdateCompletedServiceDlDto, UpdateStatusCompletedServiceDlDto>
    {

    }
}
