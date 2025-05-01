using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class RecallLeaveDto : UpdateRecallLeaveDlDto, ILinkToEntity<RecallLeave>, IDocument
{
    public Guid Id2 { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public int StatusId { get; set; }
    public string Region { get; set; }
    public string? Message { get; set; }
    new public List<RecallLeaveTableDto> Tables { get; set; }
    public List<RecallLeaveSignerDto> Signer { get; set; } = new();
    public List<ReCallLeaveFileDto> Files { get; set; } = new();
    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
