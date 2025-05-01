using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetFreeAreaByInnResponseDto:BandlikResponseDto
    {
        public GetFreeAreaByInnResultDto Result { get; set; }
    }
    public class GetFreeAreaByInnResultDto:BandlikResultDto
    {
        public List<GetFreeAreaByInnDataDto> Data { get; set; }
    }
    public class GetFreeAreaByInnDataDto
    {
        public string AllArea { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyName { get; set; }

        public string CompanyTin { get; set; }

        public string FreeArea { get; set; }

        public int Id { get; set; }

        public string Soato { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
    }

}
