using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.Memship.Doc.MemshipPaymentOrderServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderDto : UpdateMemshipPaymentOrderDlDto, ILinkToEntity<MemshipPaymentOrder>, IDocument
{
    public string Contractor { get; set; }
    public string MemshipContractNumber { get; set; }
    public DateOnly? MemshipContractDocOn { get; set; }
    public string? ServiceContractNumber { get; set; }
    public DateOnly? ServiceContractDocOn { get; set; }
    public string BankName { get; set; }
    public string BankCode { get; set; }
    public string Currency { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public int? ApplicationTypeId { get; set; }
    public string? ApplicationType { get; set; }
    public long? ServiceContractId { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? PayedAmount { get; set; }
    public decimal? RestAmount { get; set; }
    public new List<MemshipPaymentOrderFileDto> Files { get; set; } = new();
	public new List<MemshipPaymentOrderTableDto> Tables { get; set; } = new();
    #region Actions
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
