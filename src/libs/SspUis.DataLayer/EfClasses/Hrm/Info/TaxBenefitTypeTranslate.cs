using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_tax_benefit_type_translate", Schema = "hrm")]
    public partial class TaxBenefitTypeTranslate : TranslateEntity<TaxBenefitTypeTranslate, TranslateColumn>
    {

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(TaxBenefitType.Translates))]
        public virtual TaxBenefitType Owner { get; set; }
    }
}
