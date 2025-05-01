using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionDto :
    UpdateArbitrationDiscussionDlDto,
    ILinkToEntity<ArbitrationDiscussion>,
    IDocument<long>
{
    public string ContractorName { get; set; }
    public string ContractorInnPinfl { get; set; }
    public string ResponsibleContractorName { get; set; }
    public long? ResponsibleContractorId { get; set; }
    public long? ContractorId { get; set; }
    public string ResponsibleContractorInnPinfl { get; set; }
    public string ArbitrationStep { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public ArbitrationCourtApplicationDto ArbitrationCourtApplication { get; set; }

    public new List<ArbitrationDiscussionSignDto> Signs { get; set; } = new();
    public new List<ArbitrationDiscussionFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
