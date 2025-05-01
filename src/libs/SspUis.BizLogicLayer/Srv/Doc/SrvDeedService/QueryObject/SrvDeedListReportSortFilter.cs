using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class SrvDeedListReportSortFilter : SortFilterPageOptions
    {
        public bool ByRegion {  get; set; }
        public bool ByDistrict { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public DateOnly? FromDocDate { get; set; }
        public DateOnly? ToDocDate { get; set; }
    }
}
