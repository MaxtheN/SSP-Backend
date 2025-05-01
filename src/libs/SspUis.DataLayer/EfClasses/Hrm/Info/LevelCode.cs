using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_level_code", Schema = "hrm")]
    public partial class LevelCode : IHaveIdProp<int>, IHaveStateId
    {
        public LevelCode()
        {
            Translates = new HashSet<LevelCodeTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        public int? OrderCode { get; set; }
        [Required]
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Required]
        [Column("first_sign_position")]
        [StringLength(1024)]
        public string FirstSignPosition { get; set; }
        [Required]
        [Column("second_sign_position")]
        [StringLength(1024)]
        public string SecondSignPosition { get; set; }
        [Required]
        [Column("first_sign_organization")]
        [StringLength(1024)]
        public string FirstSignOrganization { get; set; }
        [Required]
        [Column("second_sign_organization")]
        [StringLength(1024)]
        public string SecondSignOrganization { get; set; }
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
        [InverseProperty(nameof(LevelCodeTranslate.Owner))]
        public virtual ICollection<LevelCodeTranslate> Translates { get; set; }
    }
}
