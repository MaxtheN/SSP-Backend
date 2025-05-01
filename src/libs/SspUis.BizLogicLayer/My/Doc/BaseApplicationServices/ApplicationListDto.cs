using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ApplicationListDto : DocumentListDto<long>, ILinkToEntity<Application>
{
    public Guid Id2 { get; set; }
    public int ApplicationTypeId { internal get; set; }
    public string DocNumber { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorAdress { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPinfl { get; set; }
    public string ContractorDirector { get; set; }
    public string ContractorPhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RegionId { get; set; }
    public string Region { get; set; }
    public int DistrictId { get; set; }
    public string District { get; set; }
    public int? CurrentStepId { get; set; }
    public string Step { get; set; }
}
