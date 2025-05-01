using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_order_to_send_business_trip_translate", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_order_to_send_business_trip_translate__lang", IsUnique = true)]
    public partial class OrderToSendBusinessTripTranslate : TranslateEntity<OrderToSendBusinessTripTranslate, TranslateColumn>
    {

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(OrderToSendBusinessTripType.Translates))]
        public virtual OrderToSendBusinessTripType Owner { get; set; }
    }
}
