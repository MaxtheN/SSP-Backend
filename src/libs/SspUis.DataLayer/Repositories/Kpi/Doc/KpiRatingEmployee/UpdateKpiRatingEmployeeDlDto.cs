using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateKpiRatingEmployeeDlDto : KpiRatingEmployeeDlDto<UpdateKpiRatingEmployeeDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
