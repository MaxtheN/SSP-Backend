using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderListDto : ILinkToEntity<MemshipPaymentOrder>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public string RegionalOrganization { get; set; }
    public string Details { get; set; }
    public decimal Amount { get; set; }
    public long? ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public long? MemshipContractId { get; set; }
    public int? BankId { get; set; }
    public int? CurrencyId { get; set; }
    public int? OrganizationId { get; set; }
    public int? StatusId { get; set; }
    public string Contractor { get; set; }
    public string MemshipContractNumber { get; set; }
    public DateOnly? MemshipContractDocOn { get; set; }
    public string BankName { get; set; }
    public string BankCode { get; set; }
    public string Currency { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public string ApplicationType { get; set; }
    public int? ApplicationTypeId { get; set; }
    public long? ServiceContractId { get; set; }
    public string? ServiceContractNumber { get; set; }
    public DateOnly? ServiceContractDocOn { get; set; }

    #region Actions
    public bool CanAccept { get; set; }
    public bool CanEdit { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }

    public bool SrvCanAccept { get; set; }
    public bool SrvCanEdit { get; set; }
    public bool SrvCanCancel { get; set; }
    public bool SrvCanDelete { get; set; }
    #endregion
}
