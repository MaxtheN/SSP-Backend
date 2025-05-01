using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveDto : UpdateEmployeeSickLeaveDlDto, ILinkToEntity<EmployeeSickLeave>, IDocument
{
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }
    public string DocNumber { get; set; }
    public string EmployeeSickLeaveType { get; set; }
    new public List<EmployeeSickLeaveTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
