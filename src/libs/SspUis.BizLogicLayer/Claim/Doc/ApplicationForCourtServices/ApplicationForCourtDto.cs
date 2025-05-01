using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Claim;

public class ApplicationForCourtDto : UpdateApplicationForCourtDlDto, ILinkToEntity<ApplicationForCourt>, IDocument
{
    public string Position { get; set; }
    public string EmployeeManage { get; set; }
    public string Department { get; set; }
    public int StatusId { get; set; }
    public string ClaimOrganization { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string Status { get; set; }
    public string Step { get; set; }
    public int? StepId { get; set; }
    public int TableId { get; set; } = TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT;
    public Guid Id2 { get; set; }
    public List<ApplicationForCourtFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
