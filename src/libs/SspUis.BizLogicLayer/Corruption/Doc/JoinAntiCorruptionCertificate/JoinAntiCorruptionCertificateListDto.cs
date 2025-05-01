using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionCertificateListDto : DocumentListDto<long>, ILinkToEntity<JoinAntiCorruptionCertificate>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public DateOnly ExpireOn { get; set; }
    public DateOnly? CancelOn { get; set; }
    public long ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public int? ContractorRegionId { get; set; }
    public string ContractorRegion { get; set; }
    public int? ContractorDistrictId { get; set; }
    public string ContractorDistrict { get; set; }
    public string ContractorFullName { get; set; }
    public string ContractorPhoneNumber { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public int? ApplicationRegionId { get; set; }
    public int? ApplicationDistrictId { get; set; }
}