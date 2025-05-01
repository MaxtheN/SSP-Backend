using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_deed_table", Schema = "srv")]
    [Index(nameof(OwnerId), Name = "ix_doc_service_deed_table__owner")]
    public partial class ServiceDeedTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("service_price_id")]
        public long? ServicePriceId { get; set; }
        [Column("service_price_table_id")]
        public long? ServicePriceTableId { get; set; }
        [Column("need_chamber_service_id")]
        public int NeedChamberServiceId { get; set; }
        [Column("offer_service_text")]
        [StringLength(500)]
        public string OfferServiceText { get; set; }
        [Column("real_coef")]
        [Precision(18, 2)]
        public decimal RealCoef { get; set; }
        [Column("price")]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Column("can_pay_divided")]
        public bool CanPayDivided { get; set; }

        [ForeignKey(nameof(NeedChamberServiceId))]
        public virtual NeedChamberService NeedChamberService { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual ServiceDeedGroup Owner { get; set; }
        [ForeignKey(nameof(ServicePriceId))]
        public virtual ServicePrice ServicePrice { get; set; }
        [ForeignKey(nameof(ServicePriceTableId))]
        public virtual ServicePriceTable ServicePriceTable { get; set; }
    }
}
