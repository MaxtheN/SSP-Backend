using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi;

[Table("doc_kpi_plan_for_employee_table", Schema = "kpi")]
[Index(nameof(OwnerId), Name = "ux_doc_kpi_plan_for_employee_table_owner")]
public partial class KpiPlanForEmployeeTable : IHaveIdProp<long>
{
    public KpiPlanForEmployeeTable()
    {
        Creates = new HashSet<KpiPlanEmployeeIndicatorCreate>();
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
    [InverseProperty(nameof(KpiPlanForEmployee.Tables))]
    public virtual KpiPlanForEmployee Owner { get; set; }
    [InverseProperty(nameof(KpiPlanEmployeeIndicatorCreate.Owner))]
    public virtual ICollection<KpiPlanEmployeeIndicatorCreate> Creates { get; set; }

    //[InverseProperty(nameof(KpiGratingId))]
    //public virtual KpiGrating KpiGrating { get; set; }
   
    //public long KpiGratingId { get; set; }
    

}
