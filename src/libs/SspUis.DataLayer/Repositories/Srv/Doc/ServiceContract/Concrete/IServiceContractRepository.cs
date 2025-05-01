using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IServiceContractRepository
        : IBaseEntityRepository<long, ServiceContract, CreateServiceContractDlDto, UpdateServiceContractDlDto, UpdateStatusServiceContractDlDto>
    {
    }
}
