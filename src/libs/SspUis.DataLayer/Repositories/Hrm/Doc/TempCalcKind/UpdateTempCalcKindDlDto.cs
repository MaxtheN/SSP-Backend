using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateTempCalcKindDlDto : TempCalcKindDlDto<UpdateTempCalcKindDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
