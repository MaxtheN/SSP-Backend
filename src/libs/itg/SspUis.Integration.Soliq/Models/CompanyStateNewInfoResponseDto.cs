using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class CompanyStateNewInfoResponseDto
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public List<CompanyStateNewInfoData> Data { get; set; }
    }
    public class CompanyStateNewInfoData
    {
        public int code { get; set; }
        public string nameUz { get; set; }
        public string nameRu { get; set; }
        public string nameLat { get; set; }
        public int countAll { get; set; }
        public int countVat { get; set; }
        public int countSt { get; set; }
        public int aaacount { get; set; }
        public int aacount { get; set; }
        public int acount { get; set; }
        public int bbbcount { get; set; }
        public int bbcount { get; set; }
        public int bcount { get; set; }
        public int ccccount { get; set; }
        public int cccount { get; set; }
        public int ccount { get; set; }
        public int dcount { get; set; }
        public int aaacountSt { get; set; }
        public int aacountSt { get; set; }
        public int acountSt { get; set; }
        public int ccccountSt { get; set; }
        public int cccountSt { get; set; }
        public int ccountSt { get; set; }
        public int bbbcountSt { get; set; }
        public int bbcountSt { get; set; }
        public int bcountSt { get; set; }
        public int dcountSt { get; set; }
        public int ccounVat { get; set; }
        public int dcounVat { get; set; }
        public int aaacounVat { get; set; }
        public int aacounVat { get; set; }
        public int acounVat { get; set; }
        public int bbbcounVat { get; set; }
        public int bbcounVat { get; set; }
        public int bcounVat { get; set; }
        public int ccccounVat { get; set; }
        public int cccounVat { get; set; }
    }
}
