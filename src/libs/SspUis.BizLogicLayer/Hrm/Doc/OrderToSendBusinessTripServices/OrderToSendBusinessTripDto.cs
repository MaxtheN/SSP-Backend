using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class OrderToSendBusinessTripDto : UpdateOrderToSendBusinessTripDlDto, ILinkToEntity<OrderToSendBusinessTrip>, IHaveIdProp<long>, IDocument
{
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string Region { get; set; }
    public string? Message { get; set; }
    public List<PersonDto> Employees { get; set; }
    new public List<OrderToSendBusinessTripTableDto> Tables { get; set; } = new();
    new public List<OrderToSendBusinessTripSignerDto> Signer { get; set; } = new();
    public List<OrderToSendBusinessTripFileDto> Files { get; set; } = new();
    public int StatusId { get; set; }
   // public int OrganizationId { get; set; }


    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
