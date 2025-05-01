using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class TariffScaleTableDlDto : EntityDto<TariffScaleTableDlDto, TariffScaleTable>, IHaveIdProp<int>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(5)]
    public string RankCode { get; set; }
    [LocalizedRequired]
    public int OrderCode { get; set; }
}
