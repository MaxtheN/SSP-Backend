using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_access_service")]
    public partial class Accessibility : IHaveIdProp<int>
    {
        public Accessibility()
        {
            SysAccessServiceHistories = new HashSet<AccessServiceHistory>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(250)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(100)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
        [Required]
        [Column("has_access")]
        public bool HasAccess { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_date", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedDate { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [InverseProperty(nameof(AccessServiceHistory.Owner))]
        public virtual ICollection<AccessServiceHistory> SysAccessServiceHistories { get; set; }
        [InverseProperty(nameof(AccessModule.Access))]
        public virtual ICollection<AccessModule> AccessModules { get; set; }
    }
}
