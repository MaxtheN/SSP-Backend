using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_indicator_department_translate", Schema = "kpi")]
    public partial class IndicatorDepartmentTranslate : TranslateEntity<IndicatorDepartmentTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(IndicatorDepartment.Translates))]
        public virtual IndicatorDepartment Owner { get; set; }
    }
}
