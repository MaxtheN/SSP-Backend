using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_rating_translate", Schema = "memship")]
public partial class RatingTranslate : TranslateEntity<RatingTranslate, TranslateColumn>
{
    

    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Rating.Translates))]
    public virtual Rating Owner { get; set; }
}
