using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_employee_turnstile_log_type", Schema = "hrm")]
public class EmployeeTurnstileLogType : IHaveStateId, IHaveIdProp<int>
{
    public EmployeeTurnstileLogType()
    {
        Translates = new HashSet<EmployeeTurnstileLogTypeTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Column("full_name")]
    [StringLength(250)]
    public string FullName { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(EmployeeTurnstileLogTypeTranslate.Owner))]
    public virtual ICollection<EmployeeTurnstileLogTypeTranslate> Translates { get; set; }
}
