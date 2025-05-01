using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestDto : UpdateSubsidyRequestDlDto, ILinkToEntity<SubsidyRequest>, IDocument
{

    //public string Organization { get; set; }
    public string Contractor { get; set; }
    public long ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPinfl { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public decimal? TotalSubsidyAmount { get; set; }
    public string ContractorSettlementAccount { get; set; }
    public string Bank { get; set; }
    public string Director { get; set; }

    public new List<SubsidyRequestTableDto> Tables { get; set; } = new();
    public new List<SubsidyRequestFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSend { get; set; }
    public bool CanRevoke { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanReject { get; set; }

    #endregion
}
