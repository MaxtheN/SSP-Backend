using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class AdditionalAgreementDto : UpdateAdditionalAgreementDlDto, ILinkToEntity<AdditionalAgreement>
{
    public Guid Id2 { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public string Contractor { get; set; }
    public string Region { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public string ApplicationType { get; set; }

    #region Actions
    public bool CanSign { get; set; }
    public bool CanReject { get; set; }
    #endregion
}
