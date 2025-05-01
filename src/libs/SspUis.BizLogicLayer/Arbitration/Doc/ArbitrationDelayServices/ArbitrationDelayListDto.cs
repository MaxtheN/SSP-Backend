using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationDelayListDto :
    DocumentListDto<long>,
    ILinkToEntity<ArbitrationDelay>
{
    public string Status { get; set; }
    public string DocNumber { get; set; }
    public DateTime DelayDate { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public long ResponsibleContractorId { get; set; }
    public string ResponsibleContractor { get; set; }
    public string ResponsibleContractorInn { get; set; }
    public long ArbitrationCourtApplicationId { get; set; }
    public int OrganizationId { get; set; }
    public string Organization { get; set; }

    #region Action
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
