namespace SspUis.BizLogicLayer.ReportServices;

public class HrmEmployeeDocumentDto
{
    public string? Region { get; set; }
    public int? OrganizationId { get; set; }
    public string OrganizationName { get; set; }

    public int? DepartmentId { get; set; }
    public string Department { get; set; }
    public string DepartmentCode { get; set; }

    public int? PositionId { get; set; }
    public string Position { get; set; }
    public string PositionCode { get; set; }

    public int? EmployeeId { get; set; }
    public string Employee { get; set; }
    public string EmployeePinfl { get; set; }

    public long TotalAppoimtEmployeesHireCount { get; set; }
    public decimal TotalAppoimtEmployeesTransferCount { get; set; }
    public long TotalAppoimtEmployeesDismissilCount { get; set; }

    public long TotalChastisementPenaltyCount { get; set; }
    public long TotalChastisementReprimandCount { get; set; }

    public long TotalTempCalcKindFinancialCount { get; set; }
    public long TotalTempCalcKindIncentiveCount { get; set; }

    public long TotalOrderToSendBusinessTripCount { get; set; }
    public long TotalOrderToSendBusinessTripCountryCount { get; set; }
    public long TotalOrderToSendBusinessTripAnotherOrgCount { get; set; }

    public long TotalEmployeeLeaveOrderCount { get; set; }

    public long TotalEmployeeSendTrainCount { get; set; }

    public long TotalEmployeeSickLeaveHomladorlikCount { get; set; }
    public long TotalEmployeeSickLeaveBolaParvarishiCount { get; set; }

    public long TotalRecallLeaveCount { get; set; }
}