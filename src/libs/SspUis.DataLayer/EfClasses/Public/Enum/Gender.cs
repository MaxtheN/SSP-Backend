using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_gender")]
    public partial class Gender : IHaveIdProp<int>
    {
        public Gender()
        {
            Translates = new HashSet<GenderTranslate>();
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

        [InverseProperty(nameof(GenderTranslate.Owner))]
        public virtual ICollection<GenderTranslate> Translates { get; set; }
        [InverseProperty(nameof(Person.Gender))]
        public virtual ICollection<Person> People { get; set; }
    }
}
