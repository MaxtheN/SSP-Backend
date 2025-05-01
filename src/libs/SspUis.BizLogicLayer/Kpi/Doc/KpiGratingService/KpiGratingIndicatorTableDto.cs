using GenericServices;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Kpi;


namespace SspUis.BizLogicLayer;

public class KpiGratingIndicatorTableDto : KpiGratingIndicatorTableDlDto, ILinkToEntity<KpiGratingIndicatorTable>
{
    public decimal? MinIndicator { get; set; }
    public decimal? MaxIndicator { get; set; }   
    public int Score { get; set; }
    public int UniteOfMeasureId { get; set; }
    public string UniteOfMeasure { get; set; }

}
