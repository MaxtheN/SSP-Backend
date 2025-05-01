using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionResultListDto : DocumentListDto<long>, ILinkToEntity<JoinAntiCorruptionResult>, IHaveIdProp<long>
{
    public string DocNumber { get; set; }
    public int ChairmenPositionId { get; set; }
    public string ChairmenFio { get; set; }
    public int Member1PositionId { get; set; }
    public string Member1Fio { get; set; }
    public int? Member2OrganizationId { get; set; }
    public int? Member2PositionId { get; set; }
    public string Member2Fio { get; set; }
    public int? Member3OrganizationId { get; set; }
    public int? Member3PositionId { get; set; }
    public string Member3Fio { get; set; }
    public int? Member4OrganizationId { get; set; }
    public int? Member4PositionId { get; set; }
    public string Member4Fio { get; set; }
    public string Status { get; set; }

    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
}
