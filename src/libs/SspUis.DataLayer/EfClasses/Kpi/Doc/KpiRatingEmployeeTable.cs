using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi
{
    [Table("doc_kpi_rating_employee_table", Schema = "kpi")]
    [Index(nameof(OwnerId), Name = "ux_doc_kpi_rating_employee_table__owner")]
    public partial class KpiRatingEmployeeTable : IHaveIdProp<long>
    {
        public KpiRatingEmployeeTable()
        {
            Points = new HashSet<KpiRatingEmployeePoint>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("employee_manage_id")]
        public long EmployeeManageId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(KpiRatingEmployee.Tables))]
        public virtual KpiRatingEmployee Owner { get; set; }
        [InverseProperty(nameof(KpiRatingEmployeePoint.Owner))]
        public virtual ICollection<KpiRatingEmployeePoint> Points { get; set; }
    }
}
