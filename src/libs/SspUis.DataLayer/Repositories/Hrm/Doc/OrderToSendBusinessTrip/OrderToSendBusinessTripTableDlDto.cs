using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class OrderToSendBusinessTripTableDlDto : EntityDto<OrderToSendBusinessTripTableDlDto, OrderToSendBusinessTripTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int BusinessTripTypeId { get; set; }
    [LocalizedRequired]
    public DateOnly BeginOn { get; set; }
    [LocalizedRequired]
    public DateOnly EndOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int? CountryId { get; set; }
    public int? RegionId { get; set; }
    public int? OrganizationId { get; set; }
    public string AnotherOrganization { get; set; }
    public string DetailForPrint { get; set; }
}
