
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class MemshipNewContractorTableDlDto : EntityDto<MemshipNewContractorTableDlDto, MemshipNewContractorsTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
  
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DistrictId { get; set; }

    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int LegalCount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int PhysicalCount { get; set; }
}
