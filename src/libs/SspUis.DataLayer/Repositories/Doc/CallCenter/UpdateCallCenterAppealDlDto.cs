using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateCallCenterAppealDlDto : CallCenterAppealDlDto<UpdateCallCenterAppealDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
