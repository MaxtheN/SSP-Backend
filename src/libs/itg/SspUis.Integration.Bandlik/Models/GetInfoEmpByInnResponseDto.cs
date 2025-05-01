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
    public class GetInfoEmpByInnResponseDto: BandlikResponseDto
    {
        public GetInfoEmpByInnResultDto Result { get; set; }
    }
    public class GetInfoEmpByInnResultDto : BandlikResultDto
    {
        public int Code { get; set; } 
        public GetInfoEmpByInnDataDto Data { get; set; }
    }
    public class GetInfoEmpByInnDataDto
    {
        public string CompanyTin { get; set; }
        public int TotalEmpMan { get; set; }
        public int TotalEmpManUnder30 { get; set; }
        public int TotalEmpWoman { get; set; }
        public int TotalEmpWomenUnder30 { get; set; }
        public int TotalPositions { get; set; }
    }
}
