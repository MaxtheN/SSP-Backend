using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Claim;

[Table("info_claim_organization_translate", Schema = "claim")]
public partial class ClaimOrganizationTranslate : TranslateEntity<ClaimOrganizationTranslate,TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ClaimOrganization.Translates))]
    public virtual ClaimOrganization Owner { get; set; }
}
