using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_business_activity_type_table")]
    public partial class BusinessActivityTypeTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("amount")]
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        [Column("currency_id")]
        public int CurrencyId { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual BusinessActivityType Owner { get; set; }
    }
}
