using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public class SignCriterionDto : UpdateSignCriterionDlDto, ILinkToEntity<SignCriterion>, IInfoHl
{
    public string State { get; set; }
    public string Position { get; set; }
    public string ContractorCategory { get; set; }
    public string OrganizationGroup { get; set; }
    public string ApplicationType { get; set; }

}
