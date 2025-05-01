using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System;
using SspUis.BizLogicLayer.MediationServices;

namespace SspUis.BizLogicLayer.Claim;

public class MediationDto : UpdateMediationDlDto, ILinkToEntity<Mediation>, IDocument
{
    public int StatusId { get; set; }
    public int? StepId { get; set; }
    public string Contractor { get; set; }
    public string PlanDocNumber { get; set; }
    public DateTime PlanDocDate { get; set; } = DateTime.Now;
    public string ContractorInn { get; set; }
    public string ClaimNeedCourt { get; set; }
    public string MediationResult { get; set; }
    public string MeetingType { get; set; }
    public int MeetingTypeId { get; set; }
    public string ClaimTheme { get; set; }
    public string Status { get; set; }
    public int ClaimThemeId { get; set; }
    public int TableId { get; set; }
    public Guid Id2 { get; set; }
    public string? Message { get; set; }
    public bool CanAccept { get; set; }
    public bool CanEdit { get; set; }
    public bool CanCancel { get; set; }
    public bool CanCreateApplicationForCourt { get; set; }

    public new List<MediationFileDto> Files { get; set; } = new();
    public List<MediationClaimApplicationTableDto> Table { get; set; } = new();
}
