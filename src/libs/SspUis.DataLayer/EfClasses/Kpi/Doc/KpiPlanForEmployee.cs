using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi;

[Table("doc_kpi_plan_for_employee", Schema = "kpi")]
public partial class KpiPlanForEmployee : IHaveIdProp<long>, IHaveStatusId
{
    public KpiPlanForEmployee()
    {
        Tables = new HashSet<KpiPlanForEmployeeTable>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("year")]
    public int? Year { get; set; }
    [Column("month")]
    public int? Month { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    
    public virtual Status Status { get; set; }
    [InverseProperty(nameof(KpiPlanForEmployeeTable.Owner))]
    public virtual ICollection<KpiPlanForEmployeeTable> Tables { get; set; }
}

