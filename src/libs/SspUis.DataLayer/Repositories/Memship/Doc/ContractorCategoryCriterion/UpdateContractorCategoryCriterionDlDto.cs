using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateContractorCategoryCriterionDlDto : ContractorCategoryCriterionDlDto<UpdateContractorCategoryCriterionDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    public long Id { get; set; }
}
