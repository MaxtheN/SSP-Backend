using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_meeting_type", Schema = "public")]
    public class MeetingType : IHaveStateId, IHaveIdProp<int>
    {
        public MeetingType()
        {
            MediationPlans = new HashSet<MediationPlan>();
            Translates = new HashSet<MeetingTypeTranslate>();
            NeedChamberServices = new HashSet<NeedChamberService>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(9)]
        public string Code { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Required]
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

        [InverseProperty(nameof(MeetingTypeTranslate.Owner))]
        public virtual ICollection<MeetingTypeTranslate> Translates { get; set; }

        [InverseProperty(nameof(MediationPlan.MeetingType))]
        public virtual ICollection<MediationPlan> MediationPlans { get; set; }

        [InverseProperty(nameof(NeedChamberService.MeetingTypes))]
        public virtual ICollection<NeedChamberService> NeedChamberServices { get; set; }
    }
}
