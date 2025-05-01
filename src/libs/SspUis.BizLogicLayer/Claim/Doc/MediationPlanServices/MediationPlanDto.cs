using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class MediationPlanDto : UpdateMediationPlanDlDto, ILinkToEntity<MediationPlan>, IDocument
{
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string MeetingType { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ClaimApplicationType { get; set; }
    public int ApplicationTypeId { get; set; }
    public string RegPhoneNumber { get; set; }
    public string ChamberPerson { get; set; }
    public string EmployeeManage { get; set; }
    public DateTime? DurationGivenPerformer { get; set; } = DateTime.Now;
    public string ClaimTheme { get; set; }
    public int ClaimThemeId { get; set; }
    public int StatusId { get; set; }
    public int? StepId { get; set; }
    public string? Message { get; set; }
    public string ApplicaionDocNumber { get; set; } 
    public DateOnly ApplicaionDocOn { get; set; } 
    public List<MediationClaimApplicationTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
