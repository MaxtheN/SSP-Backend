using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("enum_emp_appoint_order_type_translate", Schema = "hrm")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_emp_appoint_order_type_translate__lang", IsUnique = true)]
    public partial class EmpAppointOrderTypeTranslate : TranslateEntity<EmpAppointOrderTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EmpAppointOrderType.Translates))]
        public virtual EmpAppointOrderType Owner { get; set; }
    }
}
