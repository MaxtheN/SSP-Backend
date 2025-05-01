using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetTableDto : TimesheetTableDlDto, ILinkToEntity<TimesheetTable>
{
    public string Department { get; set; }
    public string Employee { get; set; }
    public string EmploymentType { get; set; }
    public string Position { get; set; }
    public string WorkSchedule { get; set; }
    new public List<TimesheetTableDayDto> TableDays { get; set; } = new();

}
