using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_district")]
    public partial class District : IHaveStateId, IHaveIdProp<int>
    {
        public District()
        {
            Contractors = new HashSet<Contractor>();
            Translates = new HashSet<DistrictTranslate>();
            Organizations = new HashSet<Organization>();
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
        [Column("soato_of_mfy")]
        [StringLength(50)]
        public string SoatoOfMfy { get; set; }
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
        [Column("region_id")]
        public int RegionId { get; set; }
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

        [ForeignKey(nameof(RegionId))]
        [InverseProperty(nameof(EfClasses.Region.Districts))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(Contractor.District))]
        public virtual ICollection<Contractor> Contractors { get; set; }
        [InverseProperty(nameof(DistrictTranslate.Owner))]
        public virtual ICollection<DistrictTranslate> Translates { get; set; }
        [InverseProperty(nameof(Organization.District))]
        public virtual ICollection<Organization> Organizations { get; set; }
        [InverseProperty(nameof(Person.BirthDistrict))]
        public virtual ICollection<Person> People { get; set; }
    }
}
