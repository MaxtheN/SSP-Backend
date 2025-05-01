using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_language")]
    public partial class Language : IHaveIdProp<int>
    {
        public Language()
        {
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(50)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
