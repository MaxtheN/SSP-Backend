using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_mfy")]
    public class Mfy :
        IHaveStateId,
        IHaveIdProp<long>
    {

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("external_id")]
        public long ExternalId { get; set; }
        [Column("external_last_updated_id")]
        public long ExternalLastUpdatedId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
    }
}
