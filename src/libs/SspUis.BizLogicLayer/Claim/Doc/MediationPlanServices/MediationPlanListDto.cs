using System;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public class MediationPlanListDto : ILinkToEntity<MediationPlan>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public int MeetingTypeId { get; set; }
    public string MeetingType { get; set; }
    public string Contractor { get; set; }
    public long ContractorId { get; set; }
    public DateTime MeditionAt { get; set; }
    public string AddressOrUrl { get; set; }
    public string ChamberPerson { get; set; }
    public long? EmployeeManageId { get; set; }
    public string EmployeeManage { get; set; }
    public int TableId { get; } = TableIdConst.CLAIM__DOC_MEDIATION_PLAN;

    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanCreateMediation { get; set; }
}
