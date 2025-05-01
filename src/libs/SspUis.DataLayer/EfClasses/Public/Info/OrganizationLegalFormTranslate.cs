using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_organization_legal_form_translate")]
    public partial class OrganizationLegalFormTranslate : TranslateEntity<OrganizationLegalFormTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(OrganizationLegalForm.Translates))]
        public virtual OrganizationLegalForm Owner { get; set; }       
    }
}
