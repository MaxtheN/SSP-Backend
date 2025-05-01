using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Edoc.Models;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealDto : UpdateCallCenterAppealDlDto, ILinkToEntity<CallCenterAppeal>, IDocument
{
    public PersonDto? Person { get; set; }
    public string AppealType { get; set; }
    public string AppealFormatType { get; set; }
    public string PersonFullName { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorCategory { get; set; }
    public string Director { get; set; }
    public string? Summary { get; set; }
    public string? Gender { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string ContractorFulName { get; set; }
    public ContractorDto Contractor { get; set; }
    public string Pinfl { get; set; }
    public string PassportSeria { get; set; }
    public string PassportNumber { get; set; }
    public string BusinessType { get; set; }
    public string Status { get; set; }
    public int StatusId { get; set; }
    public string AppealTypeArrive { get; set; }
    public string AppealDescription { get; set; }
    public string ImportOrExpot { get; set; }
    public string? Details { get; set; }
    public string Oked { get; set; }
    public int? OrganizationId { get; set; }
    public string OkedCode { get; set; }
    public ExternalIncomingDocumentDto EdocInfo { get; set; }
    public new List<CallCenterAppealFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanReject { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSendToEdoc { get; set; }
    #endregion

}
