using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_contractor_rating_translate", Schema = "memship")]
public partial class ContractorRatingTranslate : TranslateEntity<ContractorRatingTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]

    [InverseProperty(nameof(ContractorRating.Translates))]
    public virtual ContractorRating Owner { get; set; }
}
