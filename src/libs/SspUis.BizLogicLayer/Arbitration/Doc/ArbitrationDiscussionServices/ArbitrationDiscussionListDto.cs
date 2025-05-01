using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionListDto :
    DocumentListDto<long>,
    ILinkToEntity<ArbitrationDiscussion>
{
    public string Status { get; set; }
    public string DocNumber { get; set; }
    public DateTime DiscussionDate { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public long ResponsibleContractorId { get; set; }
    public string ResponsibleContractor { get; set; }
    public long ArbitrationCourtApplicationId { get; set; }

    #region Action
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
