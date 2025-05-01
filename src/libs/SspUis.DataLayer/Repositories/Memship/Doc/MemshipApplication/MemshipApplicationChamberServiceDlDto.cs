using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class MemshipApplicationChamberServiceDlDto : EntityDto<MemshipApplicationChamberServiceDlDto, MemshipApplicationChamberService>, IHaveIdProp<long>, ILinkToEntity<MemshipApplicationChamberService>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int NeedChamberServiceId { get; set; }
    }
}
