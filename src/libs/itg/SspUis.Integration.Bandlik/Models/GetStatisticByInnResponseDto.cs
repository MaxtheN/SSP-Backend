using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetStatisticByInnResponseDto: BandlikResponseDto
    {
        public GetStatisticByInnResultDto Result { get; set; }
    }
    public class GetStatisticByInnResultDto : BandlikResultDto
    {
        public GetStatisticByInnDataDto Data { get; set; }
    }
    public class GetStatisticByInnDataDto
    {
        public int OpenPositions { get; set; }

        public string OpenRates { get; set; }

        public string Tin { get; set; }

        public int TotalPositions { get; set; }

        public string TotalRates { get; set; }

        public int WorkersInSexFemale { get; set; }

        public int WorkersInSexMale { get; set; }
    }
}
