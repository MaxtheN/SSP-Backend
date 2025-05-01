using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class ArbitrationCourtApplicationDto : UpdateArbitrationCourtApplicationDlDto
    , ILinkToEntity<ArbitrationCourtApplication>
    , IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>
{
    //public long Id { get ; set ; }
    public new SspUis.BizLogicLayer.ApplicationDto Application { get; set; }
    public string ContractorResponsibleType { get; set; }
    public string ClaimResponsibleType { get; set; }
    public string Responsible { get; set; }
    public string ResponsibleInnPnfl { get; set; }
    public string ContractorInnPnfl { get; set; }
    public string Currency { get; set; }
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsCreatedByErp { get; set; }
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int? CreatedUserId { get; set; }
    public string CreatedUser { get; set; }
    public string ArbitrationCourtResult { get; set; }
    public long? ArbitrationResultId { get; set; }
    public long? ArbitrationDiscussionId { get; set; }
    public long? ArbitrationDelayId { get; set; }

    public new List<ArbitrationCourtApplicationFileDto> Files { get; set; } = new();
    public new List<ArbitrationCourtApplicationSignDto> Signer { get; set; } = new();

    #region Actions
    //public bool CanAccept { get; set; }
    //public bool CanReject { get; set; }
    //public bool CanEdit { get; set; }
    //public bool CanRevoke { get; set; }
    //public bool CanSign { get; set; }
    //public bool CanUploadSignFile { get; set; }
    public bool CanDelete { get; set; }
    public bool CanChangeStep { get; set; }
    public bool CanSetJudge { get; set; }
    public bool CanCreateDiscussionDoc { get; set; }
    public bool CanCreateDelayDoc { get; set; }
    public bool CanCreateResultDoc { get; set; }
    public bool HasArbitrationDiscussion { get; set; }
    public bool HasArbitrationDelay { get; set; }
    public bool HasArbitrationResult { get; set; }

    #endregion
}
