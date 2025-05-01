using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateContractorUnionActivityTypeDlDto : ContractorUnionActivityTypeDlDto<UpdateContractorUnionActivityTypeDlDto>, IHaveIdProp<int>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }

    }
}
