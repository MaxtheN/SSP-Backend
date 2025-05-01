using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu
{
    [Table("enum_edu_type", Schema = "dual_edu")]
    public partial class EduType : IHaveIdProp<int>
    {
        public EduType()
        {
           Translates = new HashSet<EduTypeTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
        [Column("hemis_id")]
        public int HemisId { get; set; }
        [Column("external_id")]
        public int ExternalId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [InverseProperty(nameof(EduTypeTranslate.Owner))]
        public virtual ICollection<EduTypeTranslate> Translates { get; set; }
    }
}