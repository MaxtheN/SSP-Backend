using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class UpdateStatusMemshipYearlyPlanDlDto : EntityDto<UpdateStatusMemshipYearlyPlanDlDto, MemshipYearlyPlan>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    public int StatusId { get; set; }
}
