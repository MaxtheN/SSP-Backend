using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ReceivedClaimAppDto
    {
        public Dictionary<int, string> Columns { get; set; }
        public List<ReceivedClaimAppRowsDto> Rows { get; set; }
    }

    public class ReceivedClaimAppRowsDto : BaseClaimApplicationReportDto
    {
        [JsonIgnore]
        public int? ClaimThemeId { get; set; }
        [JsonIgnore]
        public long DocumentCount { get; set; }
        public Dictionary<int, long> ClaimThemeCount { get; set; }
    }
}