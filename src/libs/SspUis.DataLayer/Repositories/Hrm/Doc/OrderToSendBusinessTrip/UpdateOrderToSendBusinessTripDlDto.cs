using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateOrderToSendBusinessTripDlDto : OrderToSendBusinessTripDlDto<UpdateOrderToSendBusinessTripDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
