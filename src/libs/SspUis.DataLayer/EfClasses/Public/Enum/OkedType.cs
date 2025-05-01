using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_oked_type")]
    public partial class OkedType
    {
        public OkedType()
        {
            Translates = new HashSet<OkedTypeTranslate>();
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
        [Column("date_of_created", TypeName = "timestamp without time zone")]
        public DateTime DateOfCreated { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("date_of_modified", TypeName = "timestamp without time zone")]
        public DateTime? DateOfModified { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(OkedTypeTranslate.Owner))]
        public virtual ICollection<OkedTypeTranslate> Translates { get; set; }
    }
}
