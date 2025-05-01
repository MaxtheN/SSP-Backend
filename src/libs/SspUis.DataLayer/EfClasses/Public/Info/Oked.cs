using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_oked")]
    [Index(nameof(Code), Name = "info_oked_unique_index_code", IsUnique = true)]
    public partial class Oked : IHaveStateId, IHaveIdProp<int>
    {
        public Oked()
        {
            Contractors = new HashSet<Contractor>();
            Translates = new HashSet<OkedTranslate>();
            Organizations = new HashSet<Organization>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(5)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(500)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("oked_type_id")]
        public int? OkedTypeId { get; set; }
        [Column("is_group")]
        public bool IsGroup { get; set; }
        [Column("parent_id")]
        public int? ParentId { get; set; }
        [Column("level")]
        public int Level { get; set; }
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

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Children))]
        public virtual Oked Parent { get; set; }
        [ForeignKey(nameof(OkedTypeId))]
        public virtual OkedType OkedType { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(Contractor.Oked))]
        public virtual ICollection<Contractor> Contractors { get; set; }
        [InverseProperty(nameof(OkedTranslate.Owner))]
        public virtual ICollection<OkedTranslate> Translates { get; set; }
        [InverseProperty(nameof(Organization.Oked))]
        public virtual ICollection<Organization> Organizations { get; set; }
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<Oked> Children { get; set; }
    }
}
