using GenericServices;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeePointDto : KpiRatingEmployeePointDlDto, ILinkToEntity<KpiRatingEmployeePoint>
{
    public string Indicator { get; set; }
    
}
