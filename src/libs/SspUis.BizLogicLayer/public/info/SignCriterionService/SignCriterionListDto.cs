using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public class SignCriterionListDto : ILinkToEntity<SignCriterion>
{
    public int Id { get; set; }
    public int StateId { get; set; }
    public int PositionId { get; set; }
    public int ContractorCategoryId { get; set; }
    public int OrganizationGroupId { get; set; }
    public int ApplicationTypeId { get; set; }
    public string State { get; set; } = null!;
    public string Position { get; set; } = null!;
    public string ContractorCategory { get; set; } = null!;
    public string OrganizationGroup { get; set; } = null!;
    public string ApplicationType { get; set; } = null!;
}
