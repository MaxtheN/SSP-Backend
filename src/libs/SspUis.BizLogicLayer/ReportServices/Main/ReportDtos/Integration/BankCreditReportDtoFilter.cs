using Newtonsoft.Json;
using SspUis.Integration.BankCredit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class BankCreditReportDtoFilter
    {
        public int? Year { get; set; }
        public string? BankMfo { get; set; }
        public bool ByRegion { get; set; } = false;
        public bool ByBank { get; set; } = false;
        public bool ByDistrict { get; set; } = false;
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
    }
}
