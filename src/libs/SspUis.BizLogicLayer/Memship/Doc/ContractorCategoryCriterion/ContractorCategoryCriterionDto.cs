using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class ContractorCategoryCriterionDto : UpdateContractorCategoryCriterionDlDto, ILinkToEntity<ContractorCategoryCriterion>, IHaveIdProp<long>, IDocument
{
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string ContractorCategory { get; set; }
}
