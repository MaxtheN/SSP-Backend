using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class OffertaCalculateInfoDtoFilter
    {
        public int? ContractTypeId { get; set; }

        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;

        public long? ContractorId { get; set; }
        public bool ByContractor { get; set; } = false;

    }
}
