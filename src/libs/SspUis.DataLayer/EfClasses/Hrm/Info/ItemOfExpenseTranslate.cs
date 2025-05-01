using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_item_of_expense_translate", Schema = "hrm")]
    //[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_enum_employment_type_translate__lang", IsUnique = true)]
    public partial class ItemOfExpenseTranslate : TranslateEntity<ItemOfExpenseTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ItemOfExpense.Translates))]
        public virtual ItemOfExpense Owner { get; set; }
    }
}
