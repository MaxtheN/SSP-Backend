using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("enum_employee_sick_leave_type_translate", Schema = "hrm")]
public partial class EmployeeSickLeaveTypeTranslate : TranslateEntity<EmployeeSickLeaveTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(EmployeeSickLeaveType.Translates))]
    public virtual EmployeeSickLeaveType Owner { get; set; }
}
