using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_service_price_type", Schema = "srv")]
    public class ServicePriceType : IHaveIdProp<int>, IHaveStateId
    {
        public ServicePriceType()
        {
            Translates = new HashSet<ServicePriceTypeTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }

        [Required]
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }

        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }

        [Required]
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(ServicePriceTypeTranslate.Owner))]
        public virtual ICollection<ServicePriceTypeTranslate> Translates { get; set; }
    }
}