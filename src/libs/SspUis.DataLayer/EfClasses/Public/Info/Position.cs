using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_position")]
    public partial class Position : IHaveStateId, IHaveIdProp<int>
    {
        public Position()
        {
            Translates = new HashSet<PositionTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(500)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("index_code")]
        [StringLength(5)]
        public string? IndexCode { get; set; }
        [Column("position_classification_id")]
        public int? PositionClassificationId { get; set; }
        [Column("position_category_id")]
        public int? PositionCategoryId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("tariff_scale_type_id")]
        public int? TariffScaleTypeId { get; set; }
        [Column("staff_type_basic_tariff_id")]
        public int? StaffTypeBasicTariffId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(PositionClassificationId))]
        public virtual PositionClassification PositionClassification { get; set; }
        [ForeignKey(nameof(PositionCategoryId))]
        public virtual PositionCategory PositionCategory { get; set; }
        [ForeignKey(nameof(StaffTypeBasicTariffId))]
        public virtual StaffTypeBasicTariff StaffTypeBasicTariff { get; set; }
        [ForeignKey(nameof(TariffScaleTypeId))]
        public virtual TariffScaleType TariffScaleType { get; set; }
        [InverseProperty(nameof(PositionTranslate.Owner))]
        public virtual ICollection<PositionTranslate> Translates { get; set; }
    }
}
