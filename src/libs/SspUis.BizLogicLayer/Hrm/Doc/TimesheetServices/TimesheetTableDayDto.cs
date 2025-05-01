using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetTableDayDto : TimesheetTableDayDlDto, ILinkToEntity<TimesheetTableDay>
{
    public string TimesheetIndicator { get; set; }
}
