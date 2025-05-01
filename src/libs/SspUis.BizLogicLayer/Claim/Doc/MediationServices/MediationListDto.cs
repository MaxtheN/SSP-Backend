using System;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public class MediationListDto : ILinkToEntity<Mediation>, IHaveIdProp<long>
{
    public Guid Id2 { get; set; }
    public long Id { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public long MediationPlanId { get; set; }
    public string ContractorDetails { get; set; }
    public string ResponsibleDetails { get; set; }
    public DateTime? CourtAt { get; set; }
    public int MediationResultId { get; set; }
    public int ClaimNeedCourtId { get; set; }
    public long ContractorId { get; set; }
    public int StatusId { get; set; }
    public string Contractor { get; set; }
    public string ClaimNeedCourt { get; set; }
    public string MediationResult { get; set; }
    public string Status { get; set; }
    public string ChamberPerson { get; set; }
    public string MediationPlanDocNumber { get; set; }
    public long? EmployeeManageId { get; set; }
    public string EmployeeManage { get; set; }
    public int TableId { get; } = TableIdConst.CLAIM__DOC_MEDIATION;
    public bool CanCancel { get; set; }
    public bool CanAccept { get; set; }
    public bool CanEdit { get; set; }
    public bool CanCreateApplicationForCourt { get; set; } = false;
}