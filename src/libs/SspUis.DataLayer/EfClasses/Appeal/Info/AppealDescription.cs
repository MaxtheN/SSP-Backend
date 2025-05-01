using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Appeal
{
    [Table("info_appeal_description", Schema = "appeal")]
    [Index(nameof(Code), Name = "info_appeal_description_ux_code", IsUnique = true)]
    public partial class AppealDescription : IHaveIdProp<int>, IHaveStateId
    {
        public AppealDescription()
        {
            Translates = new HashSet<AppealDescriptionTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("parent_id")]
        public int? ParentId { get; set; }
        [Column("has_parent")]
        public bool HasParent { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual AppealDescription? Parent { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(AppealDescriptionTranslate.Owner))]
        public virtual ICollection<AppealDescriptionTranslate> Translates { get; set; }
    }
}
