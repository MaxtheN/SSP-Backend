using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetDto : UpdateTimesheetDlDto, ILinkToEntity<Timesheet>, IDocument
{
    public string Department { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public string Status { get; set; }
    public string TimesheetType { get; set; }
    public string Organization { get; set; }
    new public List<TimesheetTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
