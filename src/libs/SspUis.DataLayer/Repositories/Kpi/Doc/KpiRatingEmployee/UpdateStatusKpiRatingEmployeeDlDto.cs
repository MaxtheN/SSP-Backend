using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateStatusKpiRatingEmployeeDlDto : EntityDto<UpdateStatusKpiRatingEmployeeDlDto, KpiRatingEmployee>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    public int StatusId { get; set; }
}
