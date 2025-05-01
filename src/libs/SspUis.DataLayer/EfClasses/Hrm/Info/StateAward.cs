using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_state_awards", Schema = "hrm")]
    public partial class StateAward : IHaveIdProp<int>, IHaveStateId
    {
        public StateAward()
        {
            EmployeeStateAwards = new HashSet<EmployeeStateAward>();
            Translates = new HashSet<StateAwardsTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(9)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(100)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(200)]
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
        [InverseProperty(nameof(EmployeeStateAward.StateAwards))]
        public virtual ICollection<EmployeeStateAward> EmployeeStateAwards { get; set; }
        [InverseProperty(nameof(StateAwardsTranslate.Owner))]
        public virtual ICollection<StateAwardsTranslate> Translates { get; set; }
    }
}
