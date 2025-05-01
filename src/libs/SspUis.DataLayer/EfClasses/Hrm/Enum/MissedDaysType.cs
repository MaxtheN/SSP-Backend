using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_missed_days_type", Schema = "hrm")]
    public partial class MissedDaysType
    {
        public MissedDaysType()
        {
            Translates = new HashSet<MissedDaysTypeTranslate>();
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
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("date_of_created", TypeName = "timestamp without time zone")]
        public DateTime DateOfCreated { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("date_of_modified", TypeName = "timestamp without time zone")]
        public DateTime? DateOfModified { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(EmployeeMissedDayTable.MissedDaysType))]
        public virtual ICollection<EmployeeMissedDayTable> EmployeeMissedDayTables { get; set; }
        [InverseProperty(nameof(MissedDaysTypeTranslate.Owner))]
        public virtual ICollection<MissedDaysTypeTranslate> Translates { get; set; }
    }
}
