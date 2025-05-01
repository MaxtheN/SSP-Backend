using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IMemshipPaymentOrderRepository : IBaseEntityRepository<long, MemshipPaymentOrder, CreateMemshipPaymentOrderDlDto, UpdateMemshipPaymentOrderDlDto, UpdateStatusMemshipPaymentOrderDlDto>
    {
    }
}
