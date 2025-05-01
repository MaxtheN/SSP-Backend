using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_order_to_send_business_trip_file", Schema = "hrm")]
public class OrderToSendBusinessTripFile : FileEntity<long>, IHaveIdProp<Guid>
{
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(OrderToSendBusinessTrip.Files))]
    public virtual OrderToSendBusinessTrip Owner { get; set; }
    [Column("is_reject")]
    public bool? IsReject { get; set; }
}