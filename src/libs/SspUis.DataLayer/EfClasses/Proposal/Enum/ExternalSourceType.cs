using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Proposal;

namespace SspUis.DataLayer.EfCode
{
    [Table("enum_external_source_type", Schema = "propos")]
    public partial class ExternalSourceType
    {
        public ExternalSourceType()
        {
            ExternalSourceTypeTranslates = new HashSet<ExternalSourceTypeTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
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
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [InverseProperty(nameof(ExternalSourceTypeTranslate.Owner))]
        public virtual ICollection<ExternalSourceTypeTranslate> ExternalSourceTypeTranslates { get; set; }
    }
}
