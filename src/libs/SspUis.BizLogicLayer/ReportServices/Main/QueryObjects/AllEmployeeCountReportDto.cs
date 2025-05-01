using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class AllEmployeeCountReportDto
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? RegionId { get; set; }
        public bool ByOrganization { get; set; } = false;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? ContractorTypeId { get; set; }
    }
}
