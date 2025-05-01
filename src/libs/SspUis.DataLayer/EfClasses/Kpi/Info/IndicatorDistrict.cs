using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_indicator_district", Schema = "kpi")]
    public class IndicatorDistrict : IHaveIdProp<int>, IHaveStateId
    {
        public IndicatorDistrict()
        {
            Tables = new HashSet<IndicatorDistrictTable>();
            Translates = new HashSet<IndicatorDistrictTranslate>();
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
        [Column("parent_id")]
        public int? ParentId { get; set; }
        [Column("unite_of_mesure_id")]
        public int? UniteOfMeasureId { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
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
        [ForeignKey(nameof(DepartmentId))]
        public virtual IndicatorDepartment Department { get; set; }
        [ForeignKey(nameof(UniteOfMeasureId))]
        public virtual UniteOfMeasure UniteOfMeasure { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Indicator Parent { get; set; }
        [InverseProperty(nameof(IndicatorDistrictTable.Owner))]
        public virtual ICollection<IndicatorDistrictTable> Tables { get; set; }
        [InverseProperty(nameof(IndicatorTranslate.Owner))]
        public virtual ICollection<IndicatorDistrictTranslate> Translates { get; set; }
    }
}
