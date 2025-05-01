using System;

namespace SspUis.BizLogicLayer.ReportServices.Main;

public class HrmEmployeeDocumentDtoFilter
{
    public bool ByOrganization { get; set; }
    public int? OrganizationId { get; set; }
    public bool ByDepartment { get; set; }
    public int? DepartmentId { get; set; }
    public bool ByPosition { get; set; }
    public int? PositionId { get; set; }
    public bool ByEmployee { get; set; }
    public int? EmployeeId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}
