using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_arbitration_judge", Schema = "arbitration")]
    public partial class ArbitrationJudge : IHaveIdProp<int>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [Column("middle_name")]
        [StringLength(100)]
        public string MiddleName { get; set; }
        [Required]
        [Column("position_name")]
        [StringLength(250)]
        public string PositionName { get; set; }
        [Required]
        [Column("organization_name")]
        [StringLength(250)]
        public string OrganizationName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("pinfl")]
        [StringLength(14)]
        public string Pinfl { get; set; }
        [Column("passport_seria")]
        [StringLength(50)]
        public string PassportSeria { get; set; }
        [Column("passport_number")]
        [StringLength(50)]
        public string PassportNumber { get; set; }
        [Column("birth_date")]
        public DateOnly? BirthDate { get; set; }
        [Column("person_id")]
        public int PersonId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }
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

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
    }
}
