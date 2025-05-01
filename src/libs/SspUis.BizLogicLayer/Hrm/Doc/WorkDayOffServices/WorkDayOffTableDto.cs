using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffTableDto : WorkDayOffTableDlDto, ILinkToEntity<WorkDayOffTable>
{
    public string Department { get; set; }
    public string Employee { get; set; }
}
