using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestListDto : ILinkToEntity<SubsidyRequest>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public int OrganizationId { get; set; }
    public int StatusId { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public decimal? TotalSubsidyAmount { get; set; }
    public string Contractor { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public long ContractorId { get; set; }
    public int RegionId { get; set; }
    public int DistrictId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ContractorInnPinfl { get; set; }

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSend { get; set; }
    public bool CanRevoke { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanReject { get; set; }

    #endregion
}
