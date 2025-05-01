using System;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileTimeReportDtoFilter
{
    public int? OrganizationId { get; set; }
    public TimeSpan? EnterTime { get; set; }
    public TimeSpan? ExitTime { get; set; }
    public string Employee { get; set; }
    public bool ByMonth { get; set; }
    public bool ByWeek { get; set; }
    public DateTime Date { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
