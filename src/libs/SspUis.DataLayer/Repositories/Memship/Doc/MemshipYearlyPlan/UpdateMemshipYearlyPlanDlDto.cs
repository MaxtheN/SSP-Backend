using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class UpdateMemshipYearlyPlanDlDto : MemshipYearlyPlanDlDto<UpdateMemshipYearlyPlanDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
