using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_organization_group_translate")]
    public partial class OrganizationGroupTranslate : EnumTranslateEntity<OrganizationGroupTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(OrganizationGroup.Translates))]
        public virtual OrganizationGroup Owner { get; set; }
    }
}
