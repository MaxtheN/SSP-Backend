using System;
using GenericServices;
using SspUis.BizLogicLayer.Appeal.Info.ExternalDocFromEdocService;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Appeal;

public class AppealApplicationListDto : ILinkToEntity<AppealApplication>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public string PersonFullName { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public string Details { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public int? PersonId { get; set; }
    public string? PersonName { get; set; }
    public bool Busyness { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public int AppealTypeId { get; set; }
    public string AppealType { get; set; }
    public int AppealFormatTypeId { get; set; }
    public string AppealFormatType { get; set; }
    public string PhoneNumber { get; set; }
    public int RegionId { get; set; }
    public string Region { get; set; }
    public int DistrictId { get; set; }
    public string District { get; set; }
    public bool OpenAppeal { get; set; }
    public int? OrganizationId { get; set; }
    public int AppealTypeArriveId { get; set; }
    public string AppealTypeArrive { get; set; }
    public int AppealDescriptionId { get; set; }
    public string AppealDescription { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public ExternalDocFromEdocDto EdocInfoForList { get; set; }
    

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanReject { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSendEdoc { get; set; }
    #endregion
}
