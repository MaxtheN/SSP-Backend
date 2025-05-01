using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IServicePriceRepository
        : IBaseEntityRepository<long, ServicePrice, CreateServicePriceDlDto, UpdateServicePriceDlDto, UpdateStatusServicePriceDlDto>
    {

    }
}
