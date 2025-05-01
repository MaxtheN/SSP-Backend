using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeMissedDayInfoDto
{
    public long EmployeeManageId { get; set; }
    public DateTime Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
