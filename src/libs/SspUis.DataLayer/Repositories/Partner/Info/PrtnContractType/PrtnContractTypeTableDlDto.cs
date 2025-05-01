using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.Models;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnContractTypeTableDlDto : EntityDto<PrtnContractTypeTableDlDto, PrtnContractTypeTable>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OrderNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int SignOrganizationTypeId { get; set; }
        public int? PositionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
        public int? OrganizationId { get; set; }
        public int? RegionId { get; set; }

    }
}
