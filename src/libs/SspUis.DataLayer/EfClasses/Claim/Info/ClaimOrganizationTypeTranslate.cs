using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Claim;

[Table("info_claim_organization_type_translate", Schema = "claim")]
[Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ux_info_claim_organization_type_translate__owner_lang", IsUnique = true)]
public partial class ClaimOrganizationTypeTranslate : TranslateEntity<ClaimOrganizationTypeTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ClaimOrganizationType.Translates))]
    public virtual ClaimOrganizationType Owner { get; set; }
}
