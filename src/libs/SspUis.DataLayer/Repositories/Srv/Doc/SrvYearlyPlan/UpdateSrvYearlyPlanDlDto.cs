using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateSrvYearlyPlanDlDto : SrvYearlyPlanDlDto<UpdateSrvYearlyPlanDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
