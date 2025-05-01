using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_sign_organization_type_translate")]
    public partial class SignOrganizationTypeTranslate : EnumTranslateEntity<SignOrganizationTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(SignOrganizationType.Translates))]
        public virtual SignOrganizationType Owner { get; set; }
    }
}
