using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffDto : UpdateWorkDayOffDlDto, ILinkToEntity<WorkDayOff>, IDocument
{
    public string Status { get; set; }
    public int StatusId { get; set; }
    public string? Message { get; set; }
    new public List<WorkDayOffTableDto> Tables { get; set; }
    public new List<WorkDayOffSignerDto> Signer { get; set; } = new();
    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
