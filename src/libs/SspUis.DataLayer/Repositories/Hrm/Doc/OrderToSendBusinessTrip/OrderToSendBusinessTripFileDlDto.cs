using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class OrderToSendBusinessTripFileDlDto :
    EntityDto<OrderToSendBusinessTripFileDlDto, OrderToSendBusinessTripFile>,
    IHaveIdProp<Guid>
{
    [LocalizedRequired]
    public Guid Id { get; set; }
    public bool? IsReject { get; set; }
}