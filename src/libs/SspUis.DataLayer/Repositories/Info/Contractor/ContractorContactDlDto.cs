using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorContactDlDto : EntityDto<ContractorContactDlDto, ContractorContact>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Contact { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ContactTypeId { get; set; }

    }
}
