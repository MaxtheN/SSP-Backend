using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_region")]
    public partial class Region : IHaveStateId, IHaveIdProp<int>
    {
        public Region()
        {
            Contractors = new HashSet<Contractor>();
            Districts = new HashSet<District>();
            Organizations = new HashSet<Organization>();
            Translates = new HashSet<RegionTranslate>();
            People = new HashSet<Person>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Column("soato")]
        [StringLength(50)]
        public string Soato { get; set; }
        [Column("roaming_code")]
        [StringLength(50)]
        public string RoamingCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")] // {"text":"bankregionid","cur":{"from":12,"to":12}}
        public int? ModifiedUserId { get; set; }
        [Column("bankregionid")]
        public int? BankRegionId { get; set; }
        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(EfClasses.Country.Regions))]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(Contractor.Region))]
        public virtual ICollection<Contractor> Contractors { get; set; }
        [InverseProperty(nameof(District.Region))]
        public virtual ICollection<District> Districts { get; set; }
        [InverseProperty(nameof(Organization.Region))]
        public virtual ICollection<Organization> Organizations { get; set; }
        [InverseProperty(nameof(RegionTranslate.Owner))]
        public virtual ICollection<RegionTranslate> Translates { get; set; }
        [InverseProperty(nameof(Person.BirthRegion))]
        public virtual ICollection<Person> People { get; set; }
    }
}
