using GenericServices;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanTableDto : MemshipYearlyPlanTableDlDto, ILinkToEntity<MemshipYearlyPlanTable>
{
    public string Region { get; set; } 
    public string District { get; set; } 
}
