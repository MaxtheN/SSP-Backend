using System;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class KpiRatingEmployeePointDlDto : EntityDto<KpiRatingEmployeePointDlDto, KpiRatingEmployeePoint>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int IndicatorId { get; set; }
    public int? Realamount { get; set; }
    public decimal? Realcount { get; set; }
    public int? Coreamount { get; set; }
    public decimal? Corecount { get; set; }
}
