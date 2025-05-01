using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices;

public class EmployeeManageDto : UpdateEmployeeManageDlDto, ILinkToEntity<EmployeeManage>
{
    public int OrganizationId { get; set; }
    public string Organization { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public string Employee { get; set; }
    public string EmployeePinfl { get; set; }
    public DateOnly EmployeeBirthOn { get; set; }
    public string EmployeePhoneNumber { get; set; }
    public string EmploymentTypeName { get; set; }
    public string DocumentInfo { get; set; } = null!;
    public long? EndDocId { get; set; }
    public int PositionClassificationId { get; set; }
}
