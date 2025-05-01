using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("info_inspection_organization_translate", Schema = "quiz")]
public partial class InspectionOrganizationTranslate : EnumTranslateEntity<InspectionOrganizationTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(InspectionOrganization.Translates))]
    public virtual InspectionOrganization Owner { get; set; }
}
