using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_application_model_code", Schema = "public")]
    public class ApplicationModelCode
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("order_code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Column("short_name")]
        [StringLength(150)]
        public string ShortName { get; set; }

        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
