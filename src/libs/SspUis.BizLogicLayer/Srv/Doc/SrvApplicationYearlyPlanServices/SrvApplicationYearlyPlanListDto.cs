using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanListDto : DocumentListDto<long>, ILinkToEntity<SrvApplicationYearlyPlan>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; }
    public string Organization { get; set; } 
    public string Region { get; set; } 
    public int? OrganizationId { get; set; } 
    public int? Year { get; set; } 
    public string Status { get; set; } 
    public int FreeCount { get; set; } 
    public int PaidCount { get; set; } 
    public decimal Amount { get; set; } 

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
