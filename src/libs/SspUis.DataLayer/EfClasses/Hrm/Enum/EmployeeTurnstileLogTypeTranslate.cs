using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_employee_turnstile_log_type_translate", Schema = "hrm")]
public class EmployeeTurnstileLogTypeTranslate : TranslateEntity<EmployeeTurnstileLogTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(EmployeeTurnstileLogType.Translates))]
    public virtual EmployeeTurnstileLogType Owner { get; set; }
}
