using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_join_anti_corruption_application_table", Schema = "corruption")]
    public partial class JoinAntiCorruptionApplicationTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("order_number")]
        public int OrderNumber { get; set; }
        [Required]
        [Column("measures")]
        [StringLength(2048)]
        public string Measures { get; set; }
        [Column("expire_on")]
        public DateOnly ExpireOn { get; set; }
        [Required]
        [Column("responsible_fio")]
        [StringLength(250)]
        public string ResponsibleFio { get; set; }
        [Required]
        [Column("measures_result")]
        [StringLength(2048)]
        public string MeasuresResult { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionApplication.Tables))]
        public virtual JoinAntiCorruptionApplication Owner { get; set; }
    }
}
