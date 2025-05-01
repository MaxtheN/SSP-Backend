using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipNewContractorDto : UpdateMemshipNewContractorDlDto, ILinkToEntity<MemshipNewContractor>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }

    public DateOnly DocOn { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string Region { get; set; }
    public int? Year { get; set; }
    public int TotalLegalCount { get; set; }
    public int TotalPhysicalCount { get; set; }

    public List<MemshipNewContractorFileDto> Files { get; set; } = new();
    
    public List<MemshipNewContractorTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
