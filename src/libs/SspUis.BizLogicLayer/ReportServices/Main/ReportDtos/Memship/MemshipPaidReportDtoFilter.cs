using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipPaidReportDtoFilter
{
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public bool? IsOld { get; set; } = true;
}
