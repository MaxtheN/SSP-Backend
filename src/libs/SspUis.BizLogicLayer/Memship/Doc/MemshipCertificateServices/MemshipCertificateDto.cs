using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateDto : UpdateMemshipCertificateDlDto, ILinkToEntity<MemshipCertificate>, IHaveIdProp<long>, IDocument
{
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public bool IsRead { get; set; }
    public int StatusId { get; set; }
    public string Organization { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorSettlementAccount { get; set; }
    public List<MemshipCertificateFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    public bool CanProlong { get; set; }
    #endregion

}
