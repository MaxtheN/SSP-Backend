using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeMissedDayTableDto : EmployeeMissedDayTableDlDto, ILinkToEntity<EmployeeMissedDayTable>
    {
        public string MissedDaysType { get; set; }
        public string Employee { get; set; }
    }
}
