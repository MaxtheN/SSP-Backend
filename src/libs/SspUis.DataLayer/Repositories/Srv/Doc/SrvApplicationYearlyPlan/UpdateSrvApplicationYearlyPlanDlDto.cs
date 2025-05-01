using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateSrvApplicationYearlyPlanDlDto : SrvApplicationYearlyPlanDlDto<UpdateSrvApplicationYearlyPlanDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
