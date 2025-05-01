using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateMassPlannedCalculationDlDto : MassPlannedCalculationDlDto<UpdateMassPlannedCalculationDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
