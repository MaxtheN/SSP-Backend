using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdatePlannedCalculationDlDto : PlannedCalculationDlDto<UpdatePlannedCalculationDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
