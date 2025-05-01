using System;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileReportByIdDtoFilter
{
    public int? OrganizationId { get; set; }
    public DateTime? OnDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? EnterTime { get; set; }
    public TimeSpan? ExitTime { get; set; }
    public int? EmployeeId { get; set; }
}
