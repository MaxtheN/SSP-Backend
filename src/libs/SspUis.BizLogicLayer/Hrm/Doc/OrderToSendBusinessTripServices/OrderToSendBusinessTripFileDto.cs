using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;

public class OrderToSendBusinessTripFileDto :
    OrderToSendBusinessTripFileDlDto, ILinkToEntity<OrderToSendBusinessTripFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}