using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer
{
   public class PrtnApplicationByPetitionInfoDto
    {
        public int? RegionId { get; set; }
        public string? RegionOrderCode { get; set; }
        public string? Region { get; set; }

        public int? DistrictId { get; set; }
        public string? District { get; set; }

        public long? MfyId { get; set; }
        public string? MFY { get; set; }
        public string? INN { get; set; }
        public string? Organization { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? DateSendedOfApplication { get; set; }
        public DateTime? DateApplicationSigned { get; set; }
        public int? DaysLateToApplicationSign { get; set; }
        public string? Status { get; set; }
        public long? StatusId { get; set; }
    }
}
