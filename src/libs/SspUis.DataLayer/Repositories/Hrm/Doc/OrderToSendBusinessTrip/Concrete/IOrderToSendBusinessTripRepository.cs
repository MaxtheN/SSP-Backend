using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface IOrderToSendBusinessTripRepository : IBaseEntityRepository<long, OrderToSendBusinessTrip, CreateOrderToSendBusinessTripDlDto, UpdateOrderToSendBusinessTripDlDto,UpdateStatusOrderToSendBusinessTripDlDto>
{
}
