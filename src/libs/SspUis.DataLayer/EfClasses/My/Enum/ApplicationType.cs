using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_application_type", Schema = "my")]
    public partial class ApplicationType : IHaveIdProp<int>
    {
        public ApplicationType()
        {
            Translates = new HashSet<ApplicationTypeTranslate>();
            Steps = new HashSet<ApplicationTypeStep>();
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
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime? CreatedAt { get; set; }
        [InverseProperty(nameof(ApplicationTypeTranslate.Owner))]
        public virtual ICollection<ApplicationTypeTranslate> Translates { get; set; }
        [InverseProperty(nameof(ApplicationTypeStep.ApplicationType))]
        public virtual ICollection<ApplicationTypeStep> Steps { get; set; }
    }
}
