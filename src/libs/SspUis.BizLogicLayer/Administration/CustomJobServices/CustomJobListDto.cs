using GenericServices;
using SspUis.BizLogicLayer;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.CustomJobServices;

public class CustomJobListDto : ILinkToEntity<CustomJob>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public DateOnly DocDate { get; set; }
    public int JobTypeId { get; set; }
    public bool IsForceUpdate { get; set; }
    public string ExtendData { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? OrganizationId { get; set; }
    public string JobType { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string District { get; set; }
    public string Region { get; set; }
    public int TotalCount { get; set; }
    public int SuccesCount { get; set; }
    public int ErrorCount {  get; set; }
}
