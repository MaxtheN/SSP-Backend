using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_indicator_department", Schema = "kpi")]
    public partial class IndicatorDepartment : IHaveIdProp<int>, IHaveStateId
    {
        public IndicatorDepartment()
        {
            Departments = new HashSet<Department>();
            Translates = new HashSet<IndicatorDepartmentTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        [StringLength(50)]
        public string Code { get; set; }
        [Column("ordercode")]
        [StringLength(250)]
        public string Ordercode { get; set; }
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
        [InverseProperty(nameof(Department.IndicatorDepartment))]
        public virtual ICollection<Department> Departments { get; set; }
        [InverseProperty(nameof(IndicatorDepartmentTranslate.Owner))]
        public virtual ICollection<IndicatorDepartmentTranslate> Translates { get; set; }
    }
}
