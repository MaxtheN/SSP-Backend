using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SspUis.DataLayer.EfClasses
{
    [Table("info_indicator_district_translate", Schema = "kpi")]
    public class IndicatorDistrictTranslate : TranslateEntity<IndicatorDistrictTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(IndicatorDistrict.Translates))]
        public virtual IndicatorDistrict Owner { get; set; }
    }
}
