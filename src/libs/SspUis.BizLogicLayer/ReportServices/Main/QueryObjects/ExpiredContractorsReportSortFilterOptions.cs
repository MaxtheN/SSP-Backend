using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices.Main.QueryObjects
{
    public class ExpiredContractorsReportSortFilterOptions
    {
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
    }
}
