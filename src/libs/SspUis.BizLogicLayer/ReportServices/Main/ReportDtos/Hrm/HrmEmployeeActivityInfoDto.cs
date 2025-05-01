namespace SspUis.BizLogicLayer.ReportServices;

public class HrmEmployeeActivityInfoDto
{
    public int? OrganizationId { get; init; }
    public string OrganizationOrderCode { get; init; }
    public string OrganizationName { get; init; }
    public long TotalEmployeesCount { get; init; }
    public decimal TotalStaffingPositionsCount { get; init; }
    public long TotalEmployeesInLeaveOrderCount { get; init; }
    public long TotalEmployeesInRecallLeaveCount { get; init; }
    public long TotalEmployeesInMaternityLeaveCount { get; init; }
    public long TotalEmployeesSendToTrainingCount { get; init; }
    public long TotalHiredEmployeesCount { get; init; }
    public long TotalDismissedEmployeesCount { get; init; }
    public long TotalTransferredEmployeesCount { get; init; }
}