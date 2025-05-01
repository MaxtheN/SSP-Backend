using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class GetCompanyHighNewInfoResponseDto
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public List<GetCompanyHighNewInfo> Data { get; set; }
    }
    public class GetCompanyHighNewInfo
    {
        public string type { get; set; }
        public int countAll { get; set; }
        public int business { get; set; }
        public int not_business { get; set; }
        public int countSt { get; set; }
        public int countVat { get; set; }
        public int sort { get; set; }
    }
}
