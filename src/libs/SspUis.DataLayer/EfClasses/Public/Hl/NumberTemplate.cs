using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("hl_number_template")]
    public partial class NumberTemplate
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("date", TypeName = "timestamp without time zone")]
        public DateTime Date { get; set; }
        [Required]
        [Column("document")]
        [StringLength(100)]
        public string Document { get; set; }
        [Required]
        [Column("template")]
        [StringLength(250)]
        public string Template { get; set; }
        //[Column("organization_id")]
        //public int OrganizationId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        //[ForeignKey(nameof(OrganizationId))]
        //public virtual Organization Organization { get; set; }
    }
}
