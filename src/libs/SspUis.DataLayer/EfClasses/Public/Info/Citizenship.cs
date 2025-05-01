using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_citizenship")]
    public partial class Citizenship : IHaveIdProp<int>, IHaveStateId
    {
        public Citizenship()
        {
            People = new HashSet<Person>();
            Translates = new HashSet<CitizenshipTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("wb_code")]
        [StringLength(50)]
        public string WbCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
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

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(Person.Citizenship))]
        public virtual ICollection<Person> People { get; set; }
        [InverseProperty(nameof(CitizenshipTranslate.Owner))]
        public virtual ICollection<CitizenshipTranslate> Translates { get; set; }
    }
}
