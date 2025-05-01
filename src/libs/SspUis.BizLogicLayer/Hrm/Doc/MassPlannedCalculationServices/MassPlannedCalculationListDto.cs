using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class MassPlannedCalculationListDto : DocumentListDto<long>, ILinkToEntity<MassPlannedCalculation>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public int? OrganizationId { get; set; } 
    public string CalculationKind { get; set; } = null!;
    public int? CalculationKindId { get; set; } 
    public string Status { get; set; } = null!; 
    public string RoundingType { get; set; } = null!;
    public int? RoundingTypeId { get; set; }
    public string OrgSettlementAccountCode { get; set; }
    public int? OrgSettlementAccountId { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
