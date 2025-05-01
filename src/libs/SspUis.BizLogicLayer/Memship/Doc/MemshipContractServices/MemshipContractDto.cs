using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GenericServices;
using SspUis.BizLogicLayer.MemshipContractServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractDto : UpdateMemshipContractDlDto, ILinkToEntity<MemshipContract>, IHaveIdProp<long>, IDocument
{
    [IgnoreWordProperty]
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public int StatusId { get; set; }
    public string Organization { get; set; }
    public string MemshipContractType { get; set; }
    public string Contractor { get; set; }
    public long ContractorId { get; set; }
    public string Director { get; set; }
    public string Address { get; set; }
    public string Inn { get; set; }
    public string Pinfl { get; set; }
    public string Bank { get; set; }
    public int BankId { get; set; }

    public string RejectMessage { get; set; }
    public DateTime RejectDate { get; set; }

    public string? OrganizationSettlementAccount { get; set; }
    public string? ContractorSettlementAccount { get; set; }
    public long? MemshipApplicationId { get; set; }
    public Guid? MemshipApplicationId2 { get; set; }
    public int TableId { get => TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT; }
    [JsonIgnore]
    public long? CurrentMemshipContractSignId { get; set; }
    public List<MemshipContractSignDto> Signs { get; set; } = new();
    public List<MemshipContractFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanSign { get; set; }
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanConfirm { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    public bool CanChangeToPaid { get; set; }
    public bool CanSelectOrganization { get; set; } = false;

    public bool CanEditFile { get; set; }
    #endregion

}
