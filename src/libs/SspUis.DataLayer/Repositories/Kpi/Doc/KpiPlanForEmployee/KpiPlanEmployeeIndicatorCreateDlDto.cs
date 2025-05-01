

using SspUis.DataLayer.EfClasses.Kpi;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiPlanEmployeeIndicatorCreateDlDto : EntityDto<KpiPlanEmployeeIndicatorCreateDlDto, KpiPlanEmployeeIndicatorCreate>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int IndicatorId { get; set; }
    public int? Amount { get; set; }
    public decimal? Count { get; set; }
}
