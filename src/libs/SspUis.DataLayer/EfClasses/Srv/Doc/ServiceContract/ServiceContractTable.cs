using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_contract_group_table", Schema = "srv")]
    public class ServiceContractTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Required]
        [Column("need_chamber_service_id")]
        public int NeedChamberServiceId { get; set; }

        [Column("service_price_id")]
        public long? ServicePriceId { get; set; }

        [Column("service_price_table_id")]
        public long? ServicePriceTableId { get; set; }

        [StringLength(500)]
        [Column("offer_service_text")]
        public string OfferServiceText { get; set; }

        [Column("real_coef")]
        public decimal RealCoef { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("can_pay_divided")]
        public bool CanPayDivided { get; set; }

        [ForeignKey(nameof(NeedChamberServiceId))]
        public virtual NeedChamberService NeedChamberService { get; set; }

        [ForeignKey(nameof(ServicePriceId))]
        public virtual ServicePrice ServicePrice { get; set; }

        [ForeignKey(nameof(ServicePriceTableId))]
        public virtual ServicePriceTable ServicePriceTable { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServiceContractGroup.Tables))]
        public virtual ServiceContractGroup Owner { get; set; }
    }
}
