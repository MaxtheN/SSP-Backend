using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.Interfaces;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.EfClasses
{
    [Table("hl_person")]
    public partial class Person : IHaveIdProp<int>, IHaveStateId, IPerson
    {
        public Person()
        {
            Employees = new HashSet<Employee>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("pinfl")]
        [StringLength(14)]
        public string Pinfl { get; set; }
        [Column("inn")]
        [StringLength(9)]
        public string Inn { get; set; }
        [Column("passport_seria")]
        [StringLength(50)]
        public string PassportSeria { get; set; }
        [Column("passport_number")]
        [StringLength(50)]
        public string PassportNumber { get; set; }
        [Column("passport_date", TypeName = "timestamp without time zone")]
        public DateTime? PassportDate { get; set; }
        [Column("passport_expiration", TypeName = "timestamp without time zone")]
        public DateTime? PassportExpiration { get; set; }
        [Required]
        [Column("surname_latin")]
        [StringLength(100)]
        public string SurnameLatin { get; set; }
        [Required]
        [Column("name_latin")]
        [StringLength(100)]
        public string NameLatin { get; set; }
        [Column("patronym_latin")]
        [StringLength(100)]
        public string PatronymLatin { get; set; }
        [Column("surname_eng")]
        [StringLength(100)]
        public string SurnameEng { get; set; }
        [Column("name_eng")]
        [StringLength(100)]
        public string NameEng { get; set; }
        [Column("short_name")]
        [StringLength(200)]
        public string ShortName { get; set; }
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("birth_date")]
        public DateOnly BirthDate { get; set; }
        [Column("gender_id")]
        public int? GenderId { get; set; }
        [Column("passport_div_name")]
        [StringLength(500)]
        public string PassportDivName { get; set; }
        [Column("birth_country_id")]
        public int? BirthCountryId { get; set; }
        [Column("birth_region_id")]
        public int? BirthRegionId { get; set; }
        [Column("birth_district_id")]
        public int? BirthDistrictId { get; set; }
        [Column("nationality_id")]
        public int? NationalityId { get; set; }
        [Column("citizenship_id")]
        public int? CitizenshipId { get; set; }
        [Column("living_region_id")]
        public int? LivingRegionId { get; set; }
        [Column("living_district_id")]
        public int? LivingDistrictId { get; set; }
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
        [Column("picture_id")]
        public Guid? PictureId { get; set; }

        [ForeignKey(nameof(BirthCountryId))]
        [InverseProperty(nameof(Country.People))]
        public virtual Country BirthCountry { get; set; }
        [ForeignKey(nameof(BirthDistrictId))]
        public virtual District BirthDistrict { get; set; }
        [ForeignKey(nameof(BirthRegionId))]
        public virtual Region BirthRegion { get; set; }
        [ForeignKey(nameof(CitizenshipId))]
        public virtual Citizenship Citizenship { get; set; }
        [ForeignKey(nameof(LivingDistrictId))]
        public virtual District LivingDistrict { get; set; }
        [ForeignKey(nameof(LivingRegionId))]
        public virtual Region LivingRegion { get; set; }
        [ForeignKey(nameof(NationalityId))]
        public virtual Nationality Nationality { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(GenderId))]
        [InverseProperty(nameof(EfClasses.Gender.People))]
        public Gender Gender { get; set; }
        [InverseProperty(nameof(Hrm.Employee.Person))]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty(nameof(User.Person))]
        public virtual ICollection<User> Users { get; set; }
    }
}
