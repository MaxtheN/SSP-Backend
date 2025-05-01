using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_landing_page_data_translate")]
    public partial class LandingPageDatumTranslate : TranslateEntity<LandingPageDatumTranslate, LandingPageDatumTranslateColumn>
    {   
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(LandingPageDatum.Translates))]
        public virtual LandingPageDatum Owner { get; set; }
    }
}
