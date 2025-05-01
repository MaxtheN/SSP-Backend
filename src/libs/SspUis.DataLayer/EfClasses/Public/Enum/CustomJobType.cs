using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_custom_job_type")]
    public partial class CustomJobType : IHaveStateId, IHaveIdProp<int>
    {
        public CustomJobType()
        {
            Translates = new HashSet<CustomJobTypeTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        
        [Required]
        [Column("code")]
        [StringLength(10)]
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
        [Column("concurrent_action_count")]
        public int ConcurrentActionCount { get; set; }

        [Required]
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(CustomJobTypeTranslate.Owner))]
        public virtual ICollection<CustomJobTypeTranslate> Translates { get; set; }
    }
}
