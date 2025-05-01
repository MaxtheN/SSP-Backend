using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipReportDtoFilter
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;
    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;
    //public bool? IsOld { get; set; } = false;
    public string Month { get; set; }
    public int? ContractorCategoryId { get; set; }
    //public bool? IsYtt { get; set;} = false;
    //public bool? IsLegal { get; set;} = false;
}
