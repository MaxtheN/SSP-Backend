using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class SaveAsExcelAllIntegrationReportByRegionDto
    {
        public string Region { get; set; }
        public long? ApplicationCount { get; set; }
        public long? PrntCertificateCount { get; set; }
        public int ? NewVacanciesCount { get; set; }


    }
}
