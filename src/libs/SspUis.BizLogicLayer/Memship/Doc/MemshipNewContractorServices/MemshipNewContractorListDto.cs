using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;


public class MemshipNewContractorListDto : DocumentListDto<long>, ILinkToEntity<MemshipNewContractor>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string Organization { get; set; }
    public int? OrganizationId { get; set; }
    public string Region { get; set; }
    public int RegionId { get; set; }
    public int? Year { get; set; }
    public int TotalLegalCount { get; set; }
    public int TotalPhysicalCount { get; set; }
    public string Status { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}