using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Info.OrganizationalStructure
{
    public class OrganizationalStructurePositionDlDto : EntityDto<OrganizationalStructurePositionDlDto, OrganizationalStructurePosition>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        ////[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionCategoryId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TariffScaleTypeId { get; set; }
        public int? TariffScaleId { get; set; }
        public int? RankId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? StaffingQuantity { get; set; }
    }
}
