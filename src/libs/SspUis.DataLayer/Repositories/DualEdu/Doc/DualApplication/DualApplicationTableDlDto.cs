using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class DualApplicationTableDlDto : EntityDto<DualApplicationTableDlDto, DualApplicationTable>, IHaveIdProp<long>, ILinkToEntity<DualApplicationTable>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(30)]
        public string OrderNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionClassificationId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int InstituteId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int SpecialtyId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EmptyPositionsCount { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1000)]
        public string Details { get; set; }

    }
}
