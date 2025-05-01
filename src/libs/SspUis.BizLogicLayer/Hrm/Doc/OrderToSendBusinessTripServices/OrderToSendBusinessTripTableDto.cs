using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using Newtonsoft.Json;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class OrderToSendBusinessTripTableDto : OrderToSendBusinessTripTableDlDto, ILinkToEntity<OrderToSendBusinessTripTable>
{

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; } // for pdf print
    public string Employee { get; set; }
    public string Position { get; set; }
    public string Department { get; set; }
    public string Country { get; set; }
    public string TableRegion { get; set; }
    public string BusinessTripType { get; set; }
    public string Organization { get; set; }
    public string DocDetails { get; set; }

}
