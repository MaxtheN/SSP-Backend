using GenericServices;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Kpi;


namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanEmployeeIndicatorCreateDto : KpiPlanEmployeeIndicatorCreateDlDto, ILinkToEntity<KpiPlanEmployeeIndicatorCreate>
{
    public long Id { get; set; }
    public int IndicatorId { get; set; }
    public string Indicator { get; set; }
    public bool IsAble { get; set; }
    public int? Amount { get; set; }
    public decimal? Count { get; set; }
}
