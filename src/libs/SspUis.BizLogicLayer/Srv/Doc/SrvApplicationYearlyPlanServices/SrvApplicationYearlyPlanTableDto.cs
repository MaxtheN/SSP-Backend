using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanTableDto : SrvApplicationYearlyPlanTableDlDto, ILinkToEntity<SrvApplicationYearlyPlanTable>
{
    public string District { get; set; }
}
