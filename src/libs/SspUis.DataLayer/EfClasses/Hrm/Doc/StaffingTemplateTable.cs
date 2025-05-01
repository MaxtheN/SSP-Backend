using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_staffing_template_table", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_doc_staffing_template_table__owner")]
    public partial class StaffingTemplateTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("position_id")]
        public int PositionId { get; set; }
        [Column("tariff_scale_type_id")]
        public int TariffScaleTypeId { get; set; }
        [Column("tariff_scale_id")]
        public int? TariffScaleId { get; set; }
        [Column("tariff_scale_table_id")]
        public int? TariffScaleTableId { get; set; }
        [Column("rank_code")]
        [StringLength(4)]
        public string RankCode { get; set; }
        [Column("rank_name")]
        [StringLength(200)]
        public string RankName { get; set; }
        [Column("quantity")]
        [Precision(18, 2)]
        public decimal? Quantity { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(StaffingTemplate.Tables))]
        public virtual StaffingTemplate Owner { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(TariffScaleId))]
        public virtual TariffScale TariffScale { get; set; }
        [ForeignKey(nameof(TariffScaleTableId))]
        public virtual TariffScaleTable TariffScaleTable { get; set; }
        [ForeignKey(nameof(TariffScaleTypeId))]
        public virtual TariffScaleType TariffScaleType { get; set; }
    }
}
