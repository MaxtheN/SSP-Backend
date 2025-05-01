using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderSortFilterOption : SortFilterPageOptions
{
    public int? CurrencyId { get; set; }
    public int? OrganizationId { get; set; }
    public int? StatusId { get; set; }
    public int? BankId { get; set; }
    public long? MemshipContractId { get; set; }
    public long? ContractorId { get; set; }
    public string? ContractorInn { get; set; }
    public DateOnly? FromDocOn { get; set; }
    public DateOnly? ToDocOn { get; set; }
    public int? ApplicationTypeId { get; set; }
    public long? ServiceContractId { get; set; }
}
