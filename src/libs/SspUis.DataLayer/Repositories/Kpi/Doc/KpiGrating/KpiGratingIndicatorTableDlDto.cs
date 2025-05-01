using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiGratingIndicatorTableDlDto : EntityDto<KpiGratingIndicatorTableDlDto, KpiGratingIndicatorTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public decimal? MinIndicator { get; set; }
    public decimal? MaxIndicator { get; set; }
    public int Score { get; set; }
    public int UniteOfMeasureId { get; set; }

}


