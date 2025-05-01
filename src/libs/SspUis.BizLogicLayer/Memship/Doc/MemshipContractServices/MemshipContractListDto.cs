using System;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractListDto : DocumentListDto<long>, ILinkToEntity<MemshipContract>, IHaveIdProp<long>, IHaveStatusId
{
    public Guid Id2 { get; set; }
    public string DocNumber { get; set; } = null!;
    public string Status { get; set; }
    public string Organization { get; set; } = null!;
    public string MemshipContractType { get; set; }
    public string Contractor { get; set; }
    public string Director { get; set; }
    public string ContractorInn { get; set; }
    public bool IsRead { get; set; }
    public string ContractorOked { get; set; }
    public string ContractorPinfl { get; set; }
    public string? OrganizationSettlementAccount { get; set; }
    public string? ContractorSettlementAccount { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public int MemshipContractTypeId { get; set; }
    public long? MemshipApplicationId { get; set; }
    public int? ContractorCategoryId { get; set; }
    public string ContractorCategory { get; set; }
    public long ContractorId { get; set; }
    public long? ContractorSettlementAccountId { get; set; }
    public long? OrganizationSettlementAccountId { get; set; }
    public int? RegionalOrganizationId { get; set; }
    //public bool IsConfirmed { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? OpfId { get; set; }
    public string? Opf { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public int TableId { get; } = TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT;

    public bool HasCertificate {  get; set; }
    /// <summary>
    /// Qiymatlari Configni ichida berildi.
    /// </summary>
    public bool HasCertificateCanceled {  get; set; }
    /// <summary>
    /// Qiymatlari Configni ichida berildi.
    /// </summary>
    #region Actions
    public bool CanSign { get; set; }
    public bool CanEdit { get; set; }
    public bool CanCancel { get; set; }
    //public bool CanReject { get; set; }
    public bool CanDelete { get; set; }
    public bool CanCreateCertificate { get; set; }
    public bool CanCreateAdditionalAgreement { get; set; }
    public bool CanChangeDocnumber { get; set; }
    #endregion
}
