using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class MassPlannedCalculationDto : UpdateMassPlannedCalculationDlDto, ILinkToEntity<MassPlannedCalculation>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public string CalculationKind { get; set; }
    public string RoundingType { get; set; }
    public string OrgSettlementAccountCode { get; set; }
    public string OrgSettlementAccount { get; set; }
    public string Department { get; set; }

    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
