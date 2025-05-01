using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanListDto : DocumentListDto<long>, ILinkToEntity<MemshipYearlyPlan>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; }
    public string Organization { get; set; } 
    public int? OrganizationId { get; set; } 
    public int? Year { get; set; } 
    public string Status { get; set; } 

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
