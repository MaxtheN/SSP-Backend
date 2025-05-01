using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;

public class AdditionalAgreementListDto : ILinkToEntity<AdditionalAgreement>
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public long ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public int ApplicationTypeId { get; set; }
    public int RegionId { get; set; }
    public int DistrictId { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public long MemshipContractId { get; set; }
    public string Contractor { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public string ApplicationType { get; set; }
    public decimal BaseFixedMinimumValue { get; set; }


    #region Actions
    public bool CanSign { get; set; }
    #endregion

}
