using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_price_group_table", Schema = "srv")]
    [Index(nameof(OwnerId), nameof(NeedChamberServiceId), Name = "ux_doc_service_price_table__ownerid_need_chamber_service_id", IsUnique = true)]
    public class ServicePriceTable : IHaveIdProp<long>, IHaveStateId
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
        [Required]
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("begin_coef")]
        public decimal? BeginCoef { get; set; }

        [Column("end_coef")]
        public decimal? EndCoef { get; set; }

        [Column("concrete_coef")]
        public decimal? ConcreteCoef { get; set; }

        [Column("is_concrete")]
        public bool IsConcrete { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ServicePriceGroup.Tables))]
        public virtual ServicePriceGroup Owner { get; set; }

        [ForeignKey(nameof(NeedChamberServiceId))]
        public virtual NeedChamberService NeedChamberService { get; set; }
    }
}