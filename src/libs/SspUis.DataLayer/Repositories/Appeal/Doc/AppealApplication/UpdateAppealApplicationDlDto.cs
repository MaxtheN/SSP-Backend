using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Appeal;

public class UpdateAppealApplicationDlDto : AppealApplicationDlDto<UpdateAppealApplicationDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
