using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_country")]
    public partial class Country : IHaveStateId, IHaveIdProp<int>
    {
        public Country()
        {
            Translates = new HashSet<CountryTranslate>();
            Contractors = new HashSet<Contractor>();
            Organizations = new HashSet<Organization>();
            Regions = new HashSet<Region>();
            People = new HashSet<Person>();
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
        [Column("text_code")]
        [StringLength(50)]
        public string TextCode { get; set; }
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
        [InverseProperty(nameof(CountryTranslate.Owner))]
        public virtual ICollection<CountryTranslate> Translates { get; set; }
        [InverseProperty(nameof(Contractor.Country))]
        public virtual ICollection<Contractor> Contractors { get; set; }
        [InverseProperty(nameof(Organization.Country))]
        public virtual ICollection<Organization> Organizations { get; set; }
        [InverseProperty(nameof(Region.Country))]
        public virtual ICollection<Region> Regions { get; set; }
        [InverseProperty(nameof(Person.BirthCountry))]
        public virtual ICollection<Person> People { get; set; }
    }
}
