using GenericServices;
using SspUis.BizLogicLayer.Kpi.Doc.KpiGratingService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class KpiGratingIndicatorDto : KpiGratingIndicatorDlDto, ILinkToEntity<KpiGratingIndicator>
{
    public string Indicator { get; set; }
    public List<KpiGratingIndicatorTableDto> Tables { get; set; } = new();
}
