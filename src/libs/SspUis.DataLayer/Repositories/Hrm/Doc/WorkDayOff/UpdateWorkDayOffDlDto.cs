using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateWorkDayOffDlDto : WorkDayOffDlDto<UpdateWorkDayOffDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
