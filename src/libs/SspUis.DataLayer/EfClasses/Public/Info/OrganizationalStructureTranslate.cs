using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure_translate")]
public partial class OrganizationalStructureTranslate : TranslateEntity<OrganizationalStructureTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(OrganizationalStructure.Translates))]
    public virtual OrganizationalStructure Owner { get; set; }
}