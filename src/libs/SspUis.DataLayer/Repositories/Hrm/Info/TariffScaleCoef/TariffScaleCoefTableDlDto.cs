using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class TariffScaleCoefTableDlDto : EntityDto<TariffScaleCoefTableDlDto, TariffScaleCoefTable>, IHaveIdProp<int>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int TariffScaleTableId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(5)]
    public string RankCode { get; set; }
    public decimal Coef { get; set; }
    public int? OrderCode { get; set; }

}
