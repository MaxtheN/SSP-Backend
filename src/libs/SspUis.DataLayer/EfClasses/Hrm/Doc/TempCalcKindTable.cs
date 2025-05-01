using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_temp_calc_kind_table", Schema = "hrm")]
public partial class TempCalcKindTable : IHaveIdProp<long>
{
	[Key]
	[Column("id")]
	public long Id { get; set; }
	[Column("owner_id")]
	public long OwnerId { get; set; }
	[Column("department_id")]
	public int DepartmentId { get; set; }
	[Column("employee_id")]
	public int EmployeeId { get; set; }
	[Column("employee_manage_id")]
	public long? EmployeeManageId { get; set; }
	[Column("percentage")]
	[Precision(18, 2)]
	public decimal Percentage { get; set; }
	[Column("amount")]
	[Precision(18, 2)]
	public decimal Amount { get; set; }
	[Column("details")]
	[StringLength(600)]
	public string Details { get; set; }
	[Column("detail_for_print")]
	public string? DetailForPrint { get; set; }
	[Column("temp_employee_manage_id")]
	public long? TempEmployeeManageId { get; set; }

	[ForeignKey(nameof(DepartmentId))]
	public virtual Department Department { get; set; }
	[ForeignKey(nameof(EmployeeId))]
	public virtual Employee Employee { get; set; }
	[ForeignKey(nameof(EmployeeManageId))]
	public virtual EmployeeManage EmployeeManage { get; set; }
	[ForeignKey(nameof(OwnerId))]
	[InverseProperty(nameof(TempCalcKind.Tables))]
	public virtual TempCalcKind Owner { get; set; }
}
